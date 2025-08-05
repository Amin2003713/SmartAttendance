using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shifty.Common.General.BaseClasses;
using Shifty.Common.Utilities.InjectionHelpers;

namespace Shifty.Application.Interfaces.Base;

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