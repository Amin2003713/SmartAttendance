using SmartAttendance.Domain.DocumentAggregate;

namespace SmartAttendance.Application.Features.Documents.Commands;

// Command: تایید یا رد سند
public sealed record ApproveDocumentCommand(Guid DocumentId) : IRequest;
public sealed record RejectDocumentCommand(Guid DocumentId) : IRequest;

public sealed class ApproveDocumentCommandHandler(IDocumentRepository repository, IUnitOfWork uow)
	: IRequestHandler<ApproveDocumentCommand>
{
	public async Task Handle(ApproveDocumentCommand request, CancellationToken cancellationToken)
	{
		var doc = await repository.GetByIdAsync(new DocumentId(request.DocumentId), cancellationToken)
			?? throw new KeyNotFoundException("سند مورد نظر یافت نشد.");
		doc.Approve();
		await uow.SaveChangesAsync(cancellationToken);
	}
}

public sealed class RejectDocumentCommandHandler(IDocumentRepository repository, IUnitOfWork uow)
	: IRequestHandler<RejectDocumentCommand>
{
	public async Task Handle(RejectDocumentCommand request, CancellationToken cancellationToken)
	{
		var doc = await repository.GetByIdAsync(new DocumentId(request.DocumentId), cancellationToken)
			?? throw new KeyNotFoundException("سند مورد نظر یافت نشد.");
		doc.Reject();
		await uow.SaveChangesAsync(cancellationToken);
	}
}

