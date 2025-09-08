using Microsoft.AspNetCore.Identity;
using SmartAttendance.Application.Features.Users.Commands.SendActivationCode;
using SmartAttendance.Application.Interfaces.Users;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General;
using SmartAttendance.Domain.Users;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Users.Commands.SendActivationCode;

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
                throw SmartAttendanceException.NotFound(localizer["User was not found."]);

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
        catch (SmartAttendanceException e)
        {
            logger.LogError(e, "Error while sending activation code.");
            throw;
        }
    }
}