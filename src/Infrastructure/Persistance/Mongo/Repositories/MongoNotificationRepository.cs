using MongoDB.Driver;
using SmartAttendance.Application.Abstractions;
using SmartAttendance.Domain.Common;
using SmartAttendance.Domain.NotificationAggregate;
using SmartAttendance.Persistence.Mongo.Context;
using SmartAttendance.Persistence.Mongo.Documents;
using SmartAttendance.Persistence.Mongo.Mappers;

namespace SmartAttendance.Persistence.Mongo.Repositories;

public sealed class MongoNotificationRepository : INotificationRepository
{
	private readonly IMongoCollection<NotificationDocument> _collection;

	public MongoNotificationRepository(MongoDbContext context)
	{
		_collection = context.Notifications;
	}

	public async Task<Notification?> GetByIdAsync(NotificationId id, CancellationToken ct = default)
	{
		var doc = await _collection.Find(x => x.Id == id.Value).FirstOrDefaultAsync(ct);
		return doc?.ToDomain();
	}

	public async Task AddAsync(Notification notification, CancellationToken ct = default)
	{
		var doc = notification.ToDocument();
		await _collection.InsertOneAsync(doc, cancellationToken: ct);
	}

	public async Task UpdateAsync(Notification notification, CancellationToken ct = default)
	{
		var doc = notification.ToDocument();
		await _collection.ReplaceOneAsync(x => x.Id == doc.Id, doc, new ReplaceOptions { IsUpsert = false }, ct);
	}

	public async Task DeleteAsync(NotificationId id, CancellationToken ct = default)
	{
		await _collection.DeleteOneAsync(x => x.Id == id.Value, ct);
	}

	public async Task<IReadOnlyList<Notification>> GetByUserAsync(UserId userId, CancellationToken ct = default)
	{
		var docs = await _collection.Find(x => x.RecipientId == userId.Value).ToListAsync(ct);
		return docs.Select(d => d.ToDomain()).ToList();
	}
}

