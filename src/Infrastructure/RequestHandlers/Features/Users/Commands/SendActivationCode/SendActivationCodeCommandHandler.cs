using Microsoft.AspNetCore.Identity;
using Shifty.Application.Features.Users.Commands.SendActivationCode;
using Shifty.Application.Interfaces.Users;
using Shifty.Common.Exceptions;
using Shifty.Common.General;
using Shifty.Domain.Users;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Users.Commands.SendActivationCode;

public class SendActivationCodeCommandHandler(
    UserManager<User> userManager,
    IdentityService identityService,
    IUserQueryRepository userRepository,
    ILogger<SendActivationCodeCommandHandler> logger,
    IStringLocalizer<SendActivationCodeCommandHandler> localizer
)
    : IRequestHandler<SendActivationCodeCommand, SendActivationCodeCommandResponse>
{
    public async Task<SendActivationCodeCommandResponse> Handle(
        SendActivationCodeCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var userId = identityService.GetUserId<Guid>();

            var user = await userRepository.GetSingleAsync(cancellationToken,
                a => a.PhoneNumber == request.PhoneNumber || a.Id == userId);


            if (user == null)
                throw IpaException.NotFound(localizer["User was not found."]);

            user.PhoneNumber = request.PhoneNumber;

            var activationCode = await userManager.GenerateTwoFactorTokenAsync(
                user,
                ApplicationConstant.Identity.CodeGenerator);

            return new SendActivationCodeCommandResponse
            {
                Success = true,
                Message = activationCode
            };
        }
        catch (IpaException e)
        {
            logger.LogError(e, "Error while sending activation code.");
            throw;
        }
    }
}