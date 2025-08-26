using Shifty.Domain.Users;

namespace Shifty.Application.Features.UserPasswords.Commands.Create;

public record CreateUserPasswordCommand(
    string Password,
    User UserId
) : IRequest;