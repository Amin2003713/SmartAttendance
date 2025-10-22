namespace SmartAttendance.Application.Features.Attachments.Commands.Delete;

public record DeleteAttachmentCommand(List<Guid> Ids) : IRequest { }