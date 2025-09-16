using Microsoft.EntityFrameworkCore;
using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Application.Interfaces.Base;

public interface IReadOnlyRepository<TEntity>
    : IScopedDependency
    where TEntity : class, IEntity
{
    DbSet<TEntity> Entities { get; }

    IQueryable<TEntity> Table { get; }

    IQueryable<TEntity> TableNoTracking { get; }

    TEntity GetById(params object[] ids);

    ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);
}