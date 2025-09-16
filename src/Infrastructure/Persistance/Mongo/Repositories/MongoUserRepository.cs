using MongoDB.Driver;
using SmartAttendance.Application.Abstractions;
using SmartAttendance.Domain.Common;
using SmartAttendance.Domain.UserAggregate;
using SmartAttendance.Persistence.Mongo.Context;
using SmartAttendance.Persistence.Mongo.Documents;
using SmartAttendance.Persistence.Mongo.Mappers;

namespace SmartAttendance.Persistence.Mongo.Repositories;

// ریپازیتوری کاربر بر بستر MongoDB
public sealed class MongoUserRepository : IUserRepository
{
	private readonly IMongoCollection<UserDocument> _collection;

	public MongoUserRepository(MongoDbContext context)
	{
		_collection = context.Users;
	}

	public async Task<UserAggregate?> GetByIdAsync(UserId id, CancellationToken ct = default)
	{
		var doc = await _collection.Find(x => x.Id == id.Value).FirstOrDefaultAsync(ct);
		return doc?.ToDomain();
	}

	public async Task AddAsync(UserAggregate user, CancellationToken ct = default)
	{
		var doc = user.ToDocument();
		await _collection.InsertOneAsync(doc, cancellationToken: ct);
	}

	public async Task UpdateAsync(UserAggregate user, CancellationToken ct = default)
	{
		var doc = user.ToDocument();
		await _collection.ReplaceOneAsync(x => x.Id == doc.Id, doc, new ReplaceOptions { IsUpsert = false }, ct);
	}

	public async Task DeleteAsync(UserId id, CancellationToken ct = default)
	{
		await _collection.DeleteOneAsync(x => x.Id == id.Value, ct);
	}
}

