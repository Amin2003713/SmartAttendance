using MongoDB.Driver;
using SmartAttendance.Application.Abstractions;
using SmartAttendance.Domain.Common;
using SmartAttendance.Persistence.Mongo.Context;
using SmartAttendance.Persistence.Mongo.Documents;

namespace SmartAttendance.Persistence.Mongo.Services;

// سرویس خواندن نقش‌ها از MongoDB
public sealed class MongoRoleReadService : IRoleReadService
{
	private readonly IMongoCollection<RoleDocument> _collection;

	public MongoRoleReadService(MongoDbContext context)
	{
		_collection = context.Roles;
	}


	public async Task<bool> RoleExistsAsync(string roleName, CancellationToken ct = default)
	{
		if (string.IsNullOrWhiteSpace(roleName)) return false;
		var normalized = roleName.Trim();
		return await _collection.Find(x => x.Name == normalized).Limit(1).AnyAsync(ct);
	}

	public async Task<(RoleId Id, string Name)?> GetByIdAsync(RoleId id, CancellationToken ct = default)
	{
		var doc = await _collection.Find(x => x.Id == id.Value).FirstOrDefaultAsync(ct);
		return doc is null ? null : (new RoleId(doc.Id), doc.Name);
	}

	public async Task<IReadOnlyList<(RoleId Id, string Name)>> ListAsync(CancellationToken ct = default)
	{
		var docs = await _collection.AsQueryable().ToListAsync(ct);
		return docs.Select(d => (new RoleId(d.Id), d.Name)).ToList();
	}
}

