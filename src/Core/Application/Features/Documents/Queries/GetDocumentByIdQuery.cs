using SmartAttendance.Application.Features.Documents.Responses;

namespace SmartAttendance.Application.Features.Documents.Queries;

public sealed record GetDocumentByIdQuery(
    Guid Id
) : IRequest<DocumentDto>;