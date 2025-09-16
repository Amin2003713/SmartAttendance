using SmartAttendance.Application.Abstractions;
using SmartAttendance.Persistence.Mongo.Context;

namespace SmartAttendance.Persistence.Mongo.UnitOfWork;

// واحد کار MongoDB (No-Op برای سازگاری)
public sealed class MongoUnitOfWork : IUnitOfWork
{
    private readonly MongoDbContext _context;

    public MongoUnitOfWork(MongoDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(0);
    }
}