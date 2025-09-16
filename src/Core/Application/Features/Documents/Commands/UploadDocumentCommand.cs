using SmartAttendance.Application.Features.Documents.Requests;
using SmartAttendance.Application.Features.Documents.Responses;
using SmartAttendance.Domain.DocumentAggregate;

namespace SmartAttendance.Application.Features.Documents.Commands;

// Command: ثبت متادیتای سند
public sealed record UploadDocumentCommand(UploadDocumentRequest Request) : IRequest<DocumentDto>;

// Handler: ثبت متادیتا
public sealed class UploadDocumentCommandHandler(IDocumentRepository repository, IUnitOfWork unitOfWork)
	: IRequestHandler<UploadDocumentCommand, DocumentDto>
{
	public async Task<DocumentDto> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
	{
		var r = request.Request;
		var doc = new Document(DocumentId.New(), r.FileName, r.ContentType, r.SizeBytes);
		await repository.AddAsync(doc, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return doc.Adapt<DocumentDto>();
	}
}

