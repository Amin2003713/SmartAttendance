using Microsoft.AspNetCore.Identity;
using SmartAttendance.Application.Features.Users.Commands.Verify;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.RequestHandlers.Features.Users.Commands.Verify;

public class VerifyCommandHandler(
    UserManager<User> userManager,
    ILogger<VerifyCommandHandler> logger,
    IStringLocalizer<VerifyCommandHandler> localizer
)
    : IRequestHandler<VerifyPhoneNumberCommand, VerifyPhoneNumberResponse>
{
    public async Task<VerifyPhoneNumberResponse> Handle(
        VerifyPhoneNumberCommand request,
        CancellationToken cancellationToken)
    {
        if (request is null)
            throw new InvalidNullInputException(nameof(request));

        try
        {
            var user = await userManager.FindByNameAsync(request.PhoneNumber);

            if (user == null)
                throw SmartAttendanceException.NotFound(additionalData: localizer["User was not found."]
                    .Value); // "کاربر یافت نشد."

            var isVerified = await VerifyTwoFactorTokenAsync(request.Code, user);

            if (isVerified)
                user.PhoneNumberConfirmed = true;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw SmartAttendanceException.BadRequest(localizer["User activation failed."]
                    .Value); // "فعال‌سازی کاربر انجام نشد."

            return new VerifyPhoneNumberResponse
            {
                IsVerified = isVerified,
                Message = isVerified
                    ? localizer["Phone number verified successfully."].Value // "شماره تلفن با موفقیت تأیید شد."
                    : localizer["Invalid code or phone number."].Value       // "کد وارد شده یا شماره تلفن نامعتبر است."
            };
        }
        catch (SmartAttendanceException e)
        {
            logger.LogError(e, "Error during phone number verification.");
            throw;
        }
    }

    private async Task<bool> VerifyTwoFactorTokenAsync(string code, User user)
    {
        try
        {
            return await userManager.VerifyTwoFactorTokenAsync(user!, ApplicationConstant.Identity.CodeGenerator, code);
        }
        catch (SmartAttendanceException e)
        {
            logger.LogError(e, "Error during two-factor token verification.");
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error during two-factor token verification.");
            throw SmartAttendanceException.InternalServerError(additionalData: localizer["Two-factor token verification failed."]
                .Value); // "احراز هویت دو مرحله‌ای ناموفق بود."
        }
    }
}