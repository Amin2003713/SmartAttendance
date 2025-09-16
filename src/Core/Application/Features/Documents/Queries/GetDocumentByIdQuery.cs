using SmartAttendance.Application.Features.Documents.Responses;

namespace SmartAttendance.Application.Features.Documents.Queries;

public sealed record GetDocumentByIdQuery(Guid Id) : IRequest<DocumentDto>;

public sealed class GetDocumentByIdQueryHandler(IDocumentRepository repo) : IRequestHandler<GetDocumentByIdQuery, DocumentDto>
{
	public async Task<DocumentDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
	{
		var doc = await repo.GetByIdAsync(new DocumentId(request.Id), cancellationToken) ?? throw new KeyNotFoundException("سند یافت نشد.");
		return doc.Adapt<DocumentDto>();
	}
}

