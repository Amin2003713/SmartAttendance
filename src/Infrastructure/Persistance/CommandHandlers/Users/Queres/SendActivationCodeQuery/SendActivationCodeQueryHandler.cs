using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shifty.Application.Common.Exceptions;
using Shifty.Application.Users.Exceptions;
using Shifty.Application.Users.Queries.SendActivationCode;
using Shifty.Common;
using Shifty.Domain.Constants;
using Shifty.Domain.Interfaces.Base;
using Shifty.Domain.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Users.Queres.SendActivationCodeQuery
{
    public class SendActivationCodeQueryHandler(UserManager<User> userManager , IRepository<User> userRepository , ILogger<SendActivationCodeQueryHandler> logger)
        : IRequestHandler<Application.Users.Queries.SendActivationCode.SendActivationCodeQuery , SendActivationCodeQueryResponse>
    {
        public async Task<SendActivationCodeQueryResponse> Handle(
            Application.Users.Queries.SendActivationCode.SendActivationCodeQuery request ,
            CancellationToken cancellationToken)
        {
            var user = await userRepository.GetSingle(a => a.PhoneNumber == request.PhoneNumber , cancellationToken);

            if (user == null)
                throw ShiftyException.NotFound(additionalData: UserExceptions.User_NotFound);

            var activationCode = await GenerateCode(user);

            if (activationCode is null)
            {
                return new SendActivationCodeQueryResponse
                {
                    Success = true , Message = "code was not generated" ,
                };
            }

            return new SendActivationCodeQueryResponse
            {
                Success = true , Message = activationCode ,
            };
        }

        private async Task<string> GenerateCode(User user)
        {
            try
            {
                return await userManager.GenerateTwoFactorTokenAsync(user , ApplicationConstant.Identity.CodeGenerator);
            }
            catch (Exception e)
            {
                logger.LogError(e.Source , e);
                throw ShiftyException.InternalServerError(additionalData: CommonExceptions.Code_Generator);
            }
        }
    }
}