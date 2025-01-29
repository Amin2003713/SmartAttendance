using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shifty.Application.Users.Command.Verify;
using Shifty.Common;
using Shifty.Common.Exceptions;
using Shifty.Domain.Constants;
using Shifty.Domain.Interfaces.Base;
using Shifty.Resources.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Domain.Features.Users;

namespace Shifty.Persistence.CommandHandlers.Users.Command.Verify
{
    public class VerifyCommandHandler(UserManager<User> userManager , IRepository<User> userRepository , ILogger<VerifyCommandHandler> logger , UserMessages messages)
        : IRequestHandler<VerifyPhoneNumberCommand , VerifyPhoneNumberResponse>
    {
        public async Task<VerifyPhoneNumberResponse> Handle(VerifyPhoneNumberCommand request , CancellationToken cancellationToken)
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            try
            {
                var user = await userRepository.GetSingle(a => a.PhoneNumber == request.PhoneNumber , cancellationToken);

                if (user == null)
                    throw ShiftyException.NotFound(additionalData: messages.User_NotFound());

                var isVerified = await VerifyTwoFactorTokenAsync(request.Code , user);

                return new VerifyPhoneNumberResponse
                {
                    IsVerified = isVerified , Message = isVerified ? "Phone number verified successfully." : "Invalid code or phone number." ,
                };
            }
            catch (ShiftyException e)
            {
                logger.LogError(e.Source , e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<bool> VerifyTwoFactorTokenAsync(string code , User user)
        {
            try
            {
                return await userManager.VerifyTwoFactorTokenAsync(user! , ApplicationConstant.Identity.CodeGenerator , code);
            }
            catch (ShiftyException e)
            {
                logger.LogError(e.Source , e);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Source , e);
                throw ShiftyException.InternalServerError(additionalData: messages.Verify_Two_Factor_Token());
            }
        }
    }
}