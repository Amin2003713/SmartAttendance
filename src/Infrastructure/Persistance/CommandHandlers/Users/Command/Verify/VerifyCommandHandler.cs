using MediatR;
using Microsoft.AspNetCore.Identity;
using Shifty.Application.Users.Command.Verify;
using Shifty.Common;
using Shifty.Common.Exceptions;
using Shifty.Domain.Constants;
using Shifty.Domain.Interfaces.Base;
using Shifty.Domain.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Users.Command.Verify
{
    public class VerifyCommandHandler(UserManager<User> userManager , IRepository<User> userRepository)
        : IRequestHandler<VerifyPhoneNumberCommand , VerifyPhoneNumberResponse>
    {
        public async Task<VerifyPhoneNumberResponse> Handle(VerifyPhoneNumberCommand request , CancellationToken cancellationToken)
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var user = await userRepository.GetSingle(a => a.PhoneNumber == request.PhoneNumber , cancellationToken);

            if (user == null)
                throw new ShiftyException(ApiResultStatusCode.NotFound , "User not found");

            var isVerified = await VerifyTwoFactorTokenAsync(request.Code , user);

            return new VerifyPhoneNumberResponse
            {
                IsVerified = isVerified , Message = isVerified ? "Phone number verified successfully." : "Invalid code or phone number." ,
            };
        }

        private async Task<bool> VerifyTwoFactorTokenAsync(string code , User user)
        {
            return await userManager.VerifyTwoFactorTokenAsync(user! , ApplicationConstant.Identity.CodeGenerator , code);
        }
    }
}