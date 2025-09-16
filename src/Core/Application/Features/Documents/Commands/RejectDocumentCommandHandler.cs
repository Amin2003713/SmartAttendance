namespace SmartAttendance.Application.Features.Documents.Commands;

public sealed class RejectDocumentCommandHandler(
    IDocumentRepository repository,
    IUnitOfWork uow
)
    : IRequestHandler<RejectDocumentCommand>
{
    public async Task Handle(RejectDocumentCommand request, CancellationToken cancellationToken)
    {
        var doc = await repository.GetByIdAsync(new DocumentId(request.DocumentId), cancellationToken) ??
                  throw new KeyNotFoundException("سند مورد نظر یافت نشد.");

        doc.Reject();
        await uow.SaveChangesAsync(cancellationToken);
    }
}