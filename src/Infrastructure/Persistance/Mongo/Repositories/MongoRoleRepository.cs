using MongoDB.Driver;
using SmartAttendance.Application.Abstractions;
using SmartAttendance.Domain.Common;
using SmartAttendance.Persistence.Mongo.Context;
using SmartAttendance.Persistence.Mongo.Documents;

namespace SmartAttendance.Persistence.Mongo.Repositories;

// ریپازیتوری نقش برای ایجاد نقش جدید
public sealed class MongoRoleRepository : IRoleRepository
{
	private readonly IMongoCollection<RoleDocument> _collection;

	public MongoRoleRepository(MongoDbContext context)
	{
		_collection = context.Roles;
	}

	public async Task<bool> ExistsByNameAsync(string roleName, CancellationToken ct = default)
		=> await _collection.Find(x => x.Name == roleName).Limit(1).AnyAsync(ct);

	public async Task AddAsync(RoleId id, string name, CancellationToken ct = default)
	{
		await _collection.InsertOneAsync(new RoleDocument { Id = id.Value, Name = name }, cancellationToken: ct);
	}
}

