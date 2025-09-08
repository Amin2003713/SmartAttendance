namespace SmartAttendance.Application.Features.Users.Commands.AddLoginRecord;

public record AddLoginRecordCommand(
    Guid UserId,
    Guid UniqueTokenIdentifier
) : IRequest;