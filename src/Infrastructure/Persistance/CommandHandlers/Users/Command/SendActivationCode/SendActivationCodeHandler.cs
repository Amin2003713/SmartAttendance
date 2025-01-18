using MediatR;
using Microsoft.AspNetCore.Identity;
using Shifty.Application.Users.Command.SendActivationCode;
using Shifty.Common;
using Shifty.Domain.Constants;
using Shifty.Domain.Interfaces.Base;
using Shifty.Domain.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Users.Command.SendActivationCode
{
    public class SendActivationCodeHandler(UserManager<User> userManager , IRepository<User> userRepository)
        : IRequestHandler<SendActivationCodeCommand , SendActivationCodeResponse>
    {
        public async Task<SendActivationCodeResponse> Handle(SendActivationCodeCommand request , CancellationToken cancellationToken)
        {
            var user = await userRepository.GetSingle(a => a.PhoneNumber == request.PhoneNumber , cancellationToken);

            if (user == null)
                throw new ShiftyException(ApiResultStatusCode.NotFound , "User not found");

            var activationCode = await GenerateCode(user);

            if (activationCode is null)
            {
                return new SendActivationCodeResponse
                {
                    Success = true , Message = "code was not generated" ,
                };
            }

            return new SendActivationCodeResponse
            {
                Success = true , Message = activationCode ,
            };
        }

        private async Task<string> GenerateCode(User user)
        {
            return await userManager.GenerateTwoFactorTokenAsync(user , ApplicationConstant.CodeGenerator);
        }
    }
}