using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shifty.Application.Users.Queries.SendActivationCode;
using Shifty.Common;
using Shifty.Domain.Constants;
using Shifty.Domain.Interfaces.Base;
using Shifty.Resources.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Common.Exceptions;
using Shifty.Domain.Features.Users;

namespace Shifty.Persistence.CommandHandlers.Users.Queres.SendActivationCodeQueryHandler
{
    public class SendActivationCodeQueryHandler(UserManager<User> userManager , IRepository<User> userRepository , ILogger<SendActivationCodeQueryHandler> logger , CommonMessages commonMessages , UserMessages userMessages)
        : IRequestHandler<SendActivationCodeQuery , SendActivationCodeQueryResponse>
    {
        public async Task<SendActivationCodeQueryResponse> Handle(
            SendActivationCodeQuery request ,
            CancellationToken cancellationToken)
        {
            var user = await userRepository.GetSingle(a => a.PhoneNumber == request.PhoneNumber , cancellationToken);

            if (user == null)
                throw ShiftyException.NotFound(additionalData: userMessages.User_NotFound());

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
                throw ShiftyException.InternalServerError(additionalData: commonMessages.Code_Generator());
            }
        }
    }
}