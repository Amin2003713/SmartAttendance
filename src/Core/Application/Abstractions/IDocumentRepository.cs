using SmartAttendance.Domain.DocumentAggregate;

namespace SmartAttendance.Application.Abstractions;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(DocumentId id, CancellationToken ct = default);
    Task            AddAsync(Document document, CancellationToken ct = default);
    Task            DeleteAsync(DocumentId id, CancellationToken ct = default);
}