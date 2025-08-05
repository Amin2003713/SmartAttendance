using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shifty.Application.Interfaces.UserPasswords;
using Shifty.Application.Users.Commands.ForgotPassword;
using Shifty.Common.Exceptions;
using Shifty.Common.General;
using Shifty.Domain.Users;

namespace Shifty.RequestHandlers.Users.Commands.ForgotPassword;

/// <summary>
///     Handles the <see cref="ForgotPasswordCommand" /> by implementing password reset logic.
/// </summary>
public class ForgotPasswordCommandHandler(
    UserManager<User> userManager,
    IUserPasswordQueryRepository PasswordQueryRepository,
    IUserPasswordCommandRepository PasswordCommandRepository,
    IStringLocalizer<ForgotPasswordCommandHandler> localizer,
    IPasswordHasher<User> Hasher,
    ILogger<ForgotPasswordCommandHandler> logger
)
    : IRequestHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var userResult = await userManager.FindByNameAsync(request.UserName);

            if (userResult == null)
                throw IpaException.NotFound(localizer["User was not found."].Value); // "کاربر یافت نشد."

            if (!await VerifyTwoFactorTokenAsync(request.Code, userResult))
                throw IpaException.Forbidden(localizer["Otp Code was invalid."]);

            var previousPasswords = await PasswordQueryRepository.TableNoTracking.Where(a => a.UserId == userResult.Id)
                .ToListAsync(cancellationToken);

            if (previousPasswords
                .Select(prev => Hasher.VerifyHashedPassword(userResult, prev.PasswordHash, request.NewPassword))
                .Any(verifyResult => verifyResult == PasswordVerificationResult.Success))
            {
                logger.LogWarning("User {UserId} tried to reuse an old password.", request.UserName);
                throw IpaException.BadRequest(localizer["Dont Reuse Old Password"]);
            }


            logger.LogInformation("Password for userId: {UserId} created successfully.", request.UserName);
            var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(userResult);

            if (passwordResetToken == null)
                throw IpaException.Forbidden(localizer["Action not allowed."].Value); // "انجام این عملیات مجاز نیست."

            var result = await userManager.ResetPasswordAsync(userResult, passwordResetToken, request.NewPassword);

            if (!result.Succeeded)
            {
                logger.LogError("Password reset failed: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));

                throw IpaException.Validation(localizer["Password change error."],
                    result.Errors); // "خطا در تغییر رمز عبور."
            }


            var userPass = new UserPassword
            {
                UserId = userResult.Id,
                PasswordHash = userResult.PasswordHash!
            };

            await PasswordCommandRepository.AddAsync(userPass, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error occurred while resetting password.");
            throw;
        }
    }


    private async Task<bool> VerifyTwoFactorTokenAsync(string code, User user)
    {
        try
        {
            return await userManager.VerifyTwoFactorTokenAsync(user!, ApplicationConstant.Identity.CodeGenerator, code);
        }
        catch (IpaException e)
        {
            logger.LogError(e, "Error during two-factor token verification.");
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error during two-factor token verification.");
            throw IpaException.InternalServerError(additionalData: localizer["Two-factor token verification failed."]
                .Value); // "احراز هویت دو مرحله‌ای ناموفق بود."
        }
    }
}