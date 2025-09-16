namespace SmartAttendance.Application.Features.Documents.Commands;

public sealed class ApproveDocumentCommandHandler(
    IDocumentRepository repository,
    IUnitOfWork uow
)
    : IRequestHandler<ApproveDocumentCommand>
{
    public async Task Handle(ApproveDocumentCommand request, CancellationToken cancellationToken)
    {
        var doc = await repository.GetByIdAsync(new DocumentId(request.DocumentId), cancellationToken) ??
                  throw new KeyNotFoundException("سند مورد نظر یافت نشد.");

        doc.Approve();
        await uow.SaveChangesAsync(cancellationToken);
    }
}