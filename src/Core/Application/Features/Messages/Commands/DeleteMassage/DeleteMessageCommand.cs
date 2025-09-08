namespace SmartAttendance.Application.Features.Messages.Commands.DeleteMassage;

public record DeleteMessageCommand(
    Guid MessageId
) : IRequest;