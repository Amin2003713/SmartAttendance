namespace SmartAttendance.Application.Features.Documents.Commands;

public sealed record RejectDocumentCommand(
    Guid DocumentId
) : IRequest;