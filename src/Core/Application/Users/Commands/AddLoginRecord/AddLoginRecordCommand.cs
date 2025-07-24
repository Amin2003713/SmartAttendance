namespace Shifty.Application.Users.Commands.AddLoginRecord;

public record AddLoginRecordCommand(
    Guid UserId,
    Guid UniqueTokenIdentifier
) : IRequest;