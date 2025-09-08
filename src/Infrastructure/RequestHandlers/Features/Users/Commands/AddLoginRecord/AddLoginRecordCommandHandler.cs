using SmartAttendance.Application.Features.Users.Commands.AddLoginRecord;
using SmartAttendance.Application.Interfaces.Jwt;
using SmartAttendance.Application.Interfaces.Users;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Users.Commands.AddLoginRecord;

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
            throw SmartAttendanceException.Unauthorized(localizer["Invalid token."]);

        if (!user.IsActive || !user.PhoneNumberConfirmed)
            throw SmartAttendanceException.Forbidden(localizer["User account is not active."]);

        var session =
            await refreshTokenQueryRepository.GetCurrentSessions(request.UniqueTokenIdentifier, cancellationToken);

        if (session is null)
            throw SmartAttendanceException.Unauthorized(localizer["Invalid token."]);

        await userCommandRepository.UpdateLastLoginDateAsync(user, cancellationToken);
    }
}