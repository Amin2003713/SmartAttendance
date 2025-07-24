using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Commons.BaseClasses;

namespace Shifty.Application.Commons.Base;

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