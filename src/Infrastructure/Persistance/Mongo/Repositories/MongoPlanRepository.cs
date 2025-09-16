using SmartAttendance.Application.Abstractions;
using SmartAttendance.Domain.Common;
using SmartAttendance.Domain.PlanAggregate;
using SmartAttendance.Persistence.Mongo.Context;
using SmartAttendance.Persistence.Mongo.Documents;
using SmartAttendance.Persistence.Mongo.Mappers;

namespace SmartAttendance.Persistence.Mongo.Repositories;

public sealed class MongoPlanRepository : IPlanRepository
{
    private readonly IMongoCollection<PlanDocument> _collection;

    public MongoPlanRepository(MongoDbContext context)
    {
        _collection = context.Plans;
    }

    public async Task<PlanAggregate?> GetByIdAsync(PlanId id, CancellationToken ct = default)
    {
        var doc = await _collection.Find(x => x.Id == id.Value).FirstOrDefaultAsync(ct);
        return doc?.ToDomain();
    }

    public async Task AddAsync(PlanAggregate plan, CancellationToken ct = default)
    {
        var doc = plan.ToDocument();
        await _collection.InsertOneAsync(doc, cancellationToken: ct);
    }

    public async Task UpdateAsync(PlanAggregate plan, CancellationToken ct = default)
    {
        var doc = plan.ToDocument();
        await _collection.ReplaceOneAsync(x => x.Id == doc.Id,
            doc,
            new ReplaceOptions
            {
                IsUpsert = false
            },
            ct);
    }

    public async Task DeleteAsync(PlanId id, CancellationToken ct = default)
    {
        await _collection.DeleteOneAsync(x => x.Id == id.Value, ct);
    }

    public async Task<IReadOnlyList<PlanAggregate>> ListAsync(CancellationToken ct = default)
    {
        var docs = await _collection.AsQueryable().ToListAsync(ct);
        return docs.Select(d => d.ToDomain()).ToList();
    }
}