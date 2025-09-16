namespace SmartAttendance.Application.Features.UserPasswords.Commands.Create;

public record CreateUserPasswordCommand(
    string Password,
    User   UserId
) : IRequest;