using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shifty.Application.Users.Command.ForgotPassword;
using Shifty.Common.Exceptions;
using Shifty.Domain.Features.Users;
using Shifty.Resources.Messages;

namespace Shifty.RequestHandlers.Users.Commands.ForgotPassword;

/// <summary>
/// Handles the <see cref="ForgotPasswordCommand"/> by implementing your password reset logic.
/// </summary>
public class ForgotPasswordCommandHandler(UserManager<User> userManager , UserMessages messages , ILogger<ForgotPasswordCommandHandler> logger) : IRequestHandler<ForgotPasswordCommand>
{
    /// <summary>
    /// Processes the <see cref="ForgotPasswordCommand"/>, applying the necessary business logic 
    /// to reset the user's password.
    /// </summary>
    /// <param name="request">The <see cref="ForgotPasswordCommand"/> object containing phone number, new password, and confirm password.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the operation.</param>
    /// <returns>An asynchronous task representing the operation.</returns>
    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        try
        {
            var userResult =await userManager.FindByNameAsync(request.UserName);

            if(userResult == null)
                throw ShiftyException.NotFound(messages.User_NotFound());

            var password = await userManager.CheckPasswordAsync( userResult , request.NewPassword);

            if (password)
                throw ShiftyException.BadRequest(messages.New_Password_Must_not_be_equal_to_Old_password());

            var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(userResult);

            if (passwordResetToken == null)
                throw ShiftyException.Forbidden(messages.NotAllowed());

            var result  = await userManager.ResetPasswordAsync(userResult , passwordResetToken,request.NewPassword);

            if(!result.Succeeded)
            {
                logger.LogError(result.Errors.ToString());
                throw ShiftyException.Validation( messages.Password_Change_Error(), result.Errors);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}