namespace SmartAttendance.Application.Features.Documents.Commands;

// Command: تایید یا رد سند
public sealed record ApproveDocumentCommand(
    Guid DocumentId
) : IRequest;