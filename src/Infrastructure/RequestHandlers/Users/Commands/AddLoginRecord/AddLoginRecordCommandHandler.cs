using Shifty.Application.Interfaces.Jwt;
using Shifty.Application.Interfaces.Users;
using Shifty.Application.Users.Commands.AddLoginRecord;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Users.Commands.AddLoginRecord;

public class AddLoginRecordCommandHandler(
    IUserCommandRepository userCommandRepository,
    IUserQueryRepository userQueryRepository,
    IRefreshTokenQueryRepository refreshTokenQueryRepository,
    IStringLocalizer<AddLoginRecordCommandHandler> localizer,
    ILogger<AddLoginRecordCommandHandler> logger
)
    : IRequestHandler<AddLoginRecordCommand>
{
    public async Task Handle(AddLoginRecordCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("<UNK> AddLoginRecordCommand request for user {UserId} , {token}",
            request.UserId!,
            request.UniqueTokenIdentifier);

        if (request.UserId == Guid.Empty)
            return;

        var user = await userQueryRepository.GetByIdAsync(cancellationToken, request.UserId);

        if (user is null)
            throw IpaException.Unauthorized(localizer["Invalid token."]);

        if (!user.IsActive || !user.PhoneNumberConfirmed)
            throw IpaException.Forbidden(localizer["User account is not active."]);

        var session =
            await refreshTokenQueryRepository.GetCurrentSessions(request.UniqueTokenIdentifier, cancellationToken);

        if (session is null)
            throw IpaException.Unauthorized(localizer["Invalid token."]);

        await userCommandRepository.UpdateLastLoginDateAsync(user, cancellationToken);
    }
}