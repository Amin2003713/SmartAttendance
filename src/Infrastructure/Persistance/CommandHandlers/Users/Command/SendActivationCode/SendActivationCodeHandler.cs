using MediatR;
using Microsoft.AspNetCore.Identity;
using Shifty.Application.Users.Command.SendActivationCode;
using Shifty.Common;
using Shifty.Domain.Constants;
using Shifty.Domain.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Users.Command.SendActivationCode
{
    public class SendActivationCodeHandler(UserManager<User> userManager) : IRequestHandler<SendActivationCodeCommand, SendActivationCodeResponse>
    {
  

        public async Task<SendActivationCodeResponse> Handle(SendActivationCodeCommand request, CancellationToken cancellationToken)
        {
            
            var user = await userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
               throw new ShiftyException(ApiResultStatusCode.NotFound , "User not found"); 
            }

            return default;
            // // Generate the activation code
            // var activationCode = GenerateActivationCode();
            //
            // // Save the activation code (assuming an ActivationCode field in ApplicationUser)
            // user.ActivationCode = activationCode;
            // user.ActivationCodeExpiry = DateTime.UtcNow.AddMinutes(15); // Example expiration time
            // var updateResult = await userManager.UpdateAsync(user);
            //
            // if (!updateResult.Succeeded)
            // {
            //     return new SendActivationCodeResponse
            //     {
            //         Success = false,
            //         Message = "Failed to save activation code."
            //     };
            // }
            //
            // // Send the activation code via email
            // var emailSent = await _emailService.SendEmailAsync(user.Email, "Activation Code", $"Your activation code is: {activationCode}");
            // if (!emailSent)
            // {
            //     return new SendActivationCodeResponse
            //     {
            //         Success = false,
            //         Message = "Failed to send activation email."
            //     };
            // }
            //
            // return new SendActivationCodeResponse
            // {
            //     Success = true,
            //     Message = "Activation code sent successfully."
            // };
        }

        private async Task<string> GenerateCode(User user)
        {
            return await userManager.GenerateTwoFactorTokenAsync(user, ApplicationConstant.CodeGenerator);
        }
    }
}
