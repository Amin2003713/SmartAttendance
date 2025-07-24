using Shifty.Domain.Users;

namespace Shifty.Application.UserPasswords.Commands.Create;

public record CreateUserPasswordCommand(
    string Password,
    User UserId
) : IRequest;