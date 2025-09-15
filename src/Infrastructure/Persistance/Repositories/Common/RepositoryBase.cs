using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Persistence.Repositories.Common;

public abstract class RepositoryBase<TEntity, TDbContext>
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    protected readonly TDbContext DbContext;
    protected readonly ILogger    Logger;

    public RepositoryBase(TDbContext dbContext, ILogger logger)
    {
        DbContext = dbContext;
        Logger    = logger;
    }


    public DbSet<TEntity> Entities => DbContext.Set<TEntity>();

    public virtual IQueryable<TEntity> Table => Entities;
    public virtual IQueryable<TEntity> TableNoTracking => Table.AsNoTracking();
}