using MongoDB.Driver;
using SmartAttendance.Application.Abstractions;
using SmartAttendance.Domain.AttendanceAggregate;
using SmartAttendance.Domain.Common;
using SmartAttendance.Persistence.Mongo.Context;
using SmartAttendance.Persistence.Mongo.Documents;
using SmartAttendance.Persistence.Mongo.Mappers;

namespace SmartAttendance.Persistence.Mongo.Repositories;

public sealed class MongoAttendanceRepository : IAttendanceRepository
{
	private readonly IMongoCollection<AttendanceDocument> _collection;

	public MongoAttendanceRepository(MongoDbContext context)
	{
		_collection = context.Attendances;
	}

	public async Task<AttendanceAggregate?> GetByIdAsync(AttendanceId id, CancellationToken ct = default)
	{
		var doc = await _collection.Find(x => x.Id == id.Value).FirstOrDefaultAsync(ct);
		return doc?.ToDomain();
	}

	public async Task AddAsync(AttendanceAggregate attendance, CancellationToken ct = default)
	{
		var doc = attendance.ToDocument();
		await _collection.InsertOneAsync(doc, cancellationToken: ct);
	}

	public async Task UpdateAsync(AttendanceAggregate attendance, CancellationToken ct = default)
	{
		var doc = attendance.ToDocument();
		await _collection.ReplaceOneAsync(x => x.Id == doc.Id, doc, new ReplaceOptions { IsUpsert = false }, ct);
	}

	public async Task DeleteAsync(AttendanceId id, CancellationToken ct = default)
	{
		await _collection.DeleteOneAsync(x => x.Id == id.Value, ct);
	}

	public async Task<AttendanceAggregate?> FindByStudentPlanAsync(UserId studentId, PlanId planId, CancellationToken ct = default)
	{
		var doc = await _collection.Find(x => x.StudentId == studentId.Value && x.PlanId == planId.Value).FirstOrDefaultAsync(ct);
		return doc?.ToDomain();
	}
}

