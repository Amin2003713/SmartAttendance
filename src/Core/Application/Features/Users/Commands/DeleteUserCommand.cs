namespace SmartAttendance.Application.Features.Users.Commands;

public sealed record DeleteUserCommand(
    Guid UserId
) : IRequest;