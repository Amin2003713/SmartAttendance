using Microsoft.EntityFrameworkCore;
using Shifty.Domain.Common.BaseClasses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Domain.IRepositories
{
    public interface IReanOnlyRepository<TEntity> where TEntity : class, IEntity
    {
        DbSet<TEntity> Entities { get; }

        IQueryable<TEntity> Table { get; }

        IQueryable<TEntity> TableNoTracking { get; }

        TEntity GetById(params object[] ids);

        ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);
    }
}