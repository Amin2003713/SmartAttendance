using SmartAttendance.Application.Abstractions;
using SmartAttendance.Domain.Common;
using SmartAttendance.Domain.DocumentAggregate;
using SmartAttendance.Persistence.Mongo.Context;
using SmartAttendance.Persistence.Mongo.Documents;
using SmartAttendance.Persistence.Mongo.Mappers;

namespace SmartAttendance.Persistence.Mongo.Repositories;

public sealed class MongoDocumentRepository : IDocumentRepository
{
    private readonly IMongoCollection<DocumentDocument> _collection;

    public MongoDocumentRepository(MongoDbContext context)
    {
        _collection = context.Documents;
    }

    public async Task<Document?> GetByIdAsync(DocumentId id, CancellationToken ct = default)
    {
        var doc = await _collection.Find(x => x.Id == id.Value).FirstOrDefaultAsync(ct);
        return doc?.ToDomain();
    }

    public async Task AddAsync(Document document, CancellationToken ct = default)
    {
        var doc = document.ToDocument();
        await _collection.InsertOneAsync(doc, cancellationToken: ct);
    }

    public async Task DeleteAsync(DocumentId id, CancellationToken ct = default)
    {
        await _collection.DeleteOneAsync(x => x.Id == id.Value, ct);
    }
}