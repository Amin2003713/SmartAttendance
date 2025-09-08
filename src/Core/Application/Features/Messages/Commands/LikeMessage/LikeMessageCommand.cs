namespace SmartAttendance.Application.Features.Messages.Commands.LikeMessage;

public record LikeMessageCommand(
    Guid MessageId
) : IRequest;