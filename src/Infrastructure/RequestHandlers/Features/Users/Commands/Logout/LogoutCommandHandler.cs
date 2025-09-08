using SmartAttendance.Application.Features.Users.Commands.Logout;
using SmartAttendance.Application.Interfaces.Jwt;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Users.Commands.Logout;

public class LogoutCommandHandler(
    IRefreshTokenQueryRepository queryRepository,
    IRefreshTokenCommandRepository commandRepository,
    IdentityService identityService
)
    : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!identityService.IsAuthenticated)
            return;

        var sessions =
            await queryRepository.GetCurrentSessions(identityService.GetUniqueId()!.Value, cancellationToken);

        if (sessions is null)
            return;

        await commandRepository.RevokeSecondarySession([sessions], cancellationToken);
    }
}