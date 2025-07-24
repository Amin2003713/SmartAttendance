using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shifty.Domain.Commons.BaseClasses;

namespace Shifty.Application.Commons.Base;

public interface IQueryRepository<TEntity>
    where TEntity : class, IEntity
{
    DbSet<TEntity> Entities { get; }
    IQueryable<TEntity> Table { get; }
    IQueryable<TEntity> TableNoTracking { get; }

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    bool       Any(Expression<Func<TEntity, bool>> predicate);

    TEntity            GetById(params object[] ids);
    ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    Task<TEntity> GetSingleAsync(
        CancellationToken cancellationToken,
        Expression<Func<TEntity, bool>> predicate = null!);

    Task<TEntity> FirstOrDefaultsAsync(
        Expression<Func<TEntity, bool>> predicate = null!,
        CancellationToken cancellationToken = default);

    TEntity GetSingle(Expression<Func<TEntity, bool>> predicate = null!);

    void LoadCollection<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
        where TProperty : class;

    Task LoadCollectionAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty,
        CancellationToken cancellationToken)
        where TProperty : class;

    void LoadReference<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty>> referenceProperty)
        where TProperty : class;

    Task LoadReferenceAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty>> referenceProperty,
        CancellationToken cancellationToken)
        where TProperty : class;


    void LoadCollection<TProperty>(
        IEnumerable<TEntity> entities,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
        where TProperty : class;

    Task LoadCollectionAsync<TProperty>(
        IEnumerable<TEntity> entities,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty,
        CancellationToken cancellationToken)
        where TProperty : class;

    void LoadReference<TProperty>(
        IEnumerable<TEntity> entities,
        Expression<Func<TEntity, TProperty>> referenceProperty)
        where TProperty : class;

    Task LoadReferenceAsync<TProperty>(
        IEnumerable<TEntity> entities,
        Expression<Func<TEntity, TProperty>> referenceProperty,
        CancellationToken cancellationToken)
        where TProperty : class;
}

public interface IGenericSeeder<in TDbContext>
{
    Task SeedAsync(TDbContext dbContext, CancellationToken cancellationToken);
}