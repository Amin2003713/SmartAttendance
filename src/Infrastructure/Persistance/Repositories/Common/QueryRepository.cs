using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Persistence.Repositories.Common;

public class QueryRepository<TEntity>(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<TEntity>> logger
)
    : RepositoryBase<TEntity, ReadOnlyDbContext>(dbContext, logger),
        IQueryRepository<TEntity>
    where TEntity : class, IEntity
{
    public virtual TEntity GetById(params object[] ids)
    {
        try
        {
            Logger.LogInformation("Fetching entity of type {EntityType}", typeof(TEntity).Name);
            var entity = Entities.Find(ids);
            if (entity != null)
                return entity;

            Logger.LogWarning("Entity of type {EntityType} not found", typeof(TEntity).Name);
            return null!;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error fetching entity of type {EntityType}", typeof(TEntity).Name);
            return null!;
        }
    }

    public bool Any(Expression<Func<TEntity, bool>> predicate)
    {
        return TableNoTracking.Any(predicate);
    }

    public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate = null!)
    {
        try
        {
            Logger.LogInformation("Fetching single entity of type {EntityType} with predicate", typeof(TEntity).Name);
            return (predicate is null
                ? TableNoTracking.SingleOrDefault()
                : TableNoTracking.SingleOrDefault(predicate)!)!;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error fetching single entity of type {EntityType}", typeof(TEntity).Name);
            return null!;
        }
    }

    public void LoadCollection<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
        where TProperty : class
    {
        try
        {
            Logger.LogInformation("Loading collection property for entity of type {EntityType}", typeof(TEntity).Name);
            var collection = DbContext.Entry(entity).Collection(collectionProperty);

            if (!collection.IsLoaded)
            {
                collection.Load();
                Logger.LogInformation("Collection property loaded for entity of type {EntityType}",
                    typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,
                "Error loading collection property for entity of type {EntityType}",
                typeof(TEntity).Name);

            throw SmartAttendanceException.InternalServerError();
        }
    }

    public void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty)
        where TProperty : class
    {
        try
        {
            Logger.LogInformation("Loading reference property for entity of type {EntityType}", typeof(TEntity).Name);
            var reference = DbContext.Entry(entity).Reference(referenceProperty);

            if (!reference.IsLoaded)
            {
                reference.Load();
                Logger.LogInformation("Reference property loaded for entity of type {EntityType}",
                    typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,
                "Error loading reference property for entity of type {EntityType}",
                typeof(TEntity).Name);

            throw SmartAttendanceException.InternalServerError();
        }
    }

    public void LoadCollection<TProperty>(
        IEnumerable<TEntity> entities,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
        where TProperty : class
    {
        foreach (var entity in entities)
        {
            DbContext.Entry(entity).Collection(collectionProperty).Load();
        }
    }

    public void LoadReference<TProperty>(
        IEnumerable<TEntity> entities,
        Expression<Func<TEntity, TProperty>> referenceProperty)
        where TProperty : class
    {
        foreach (var entity in entities)
        {
            DbContext.Entry(entity).Reference(referenceProperty).Load();
        }
    }

    public virtual async ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
    {
        try
        {
            Logger.LogInformation("Fetching entity of type {EntityType}", typeof(TEntity).Name);
            var entity = await Entities.FindAsync(ids, cancellationToken).ConfigureAwait(false);
            if (entity == null) Logger.LogWarning("Entity of type {EntityType} not found", typeof(TEntity).Name);

            return entity;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error fetching entity of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.NotFound();
        }
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await TableNoTracking.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<TEntity> GetSingleAsync(
        CancellationToken cancellationToken,
        Expression<Func<TEntity, bool>> predicate = null!)
    {
        try
        {
            Logger.LogInformation("Fetching single entity of type {EntityType} with predicate", typeof(TEntity).Name);
            return (predicate is null
                       ? await TableNoTracking.SingleOrDefaultAsync(cancellationToken)
                       : await TableNoTracking.SingleOrDefaultAsync(predicate, cancellationToken)) ??
                   null!;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error fetching single entity of type {EntityType}", typeof(TEntity).Name);
            return null!;
        }
    }

    public Task<TEntity> FirstOrDefaultsAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Fetching single entity of type {EntityType} with predicate", typeof(TEntity).Name);
            return (predicate is null
                ? TableNoTracking.FirstOrDefaultAsync(cancellationToken)
                : TableNoTracking.FirstOrDefaultAsync(predicate, cancellationToken)!)!;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error fetching single entity of type {EntityType}", typeof(TEntity).Name);
            return null!;
        }
    }

    public async Task LoadCollectionAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty,
        CancellationToken cancellationToken)
        where TProperty : class
    {
        try
        {
            Logger.LogInformation("Loading collection property for entity of type {EntityType}", typeof(TEntity).Name);
            var collection = DbContext.Entry(entity).Collection(collectionProperty);

            if (!collection.IsLoaded)
            {
                await collection.LoadAsync(cancellationToken).ConfigureAwait(false);
                Logger.LogInformation("Collection property loaded for entity of type {EntityType}",
                    typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,
                "Error loading collection property for entity of type {EntityType}",
                typeof(TEntity).Name);

            throw SmartAttendanceException.InternalServerError();
        }
    }

    public async Task LoadReferenceAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty>> referenceProperty,
        CancellationToken cancellationToken)
        where TProperty : class
    {
        try
        {
            Logger.LogInformation("Loading reference property for entity of type {EntityType}", typeof(TEntity).Name);
            var reference = DbContext.Entry(entity).Reference(referenceProperty);

            if (!reference.IsLoaded)
            {
                await reference.LoadAsync(cancellationToken).ConfigureAwait(false);
                Logger.LogInformation("Reference property loaded for entity of type {EntityType}",
                    typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,
                "Error loading reference property for entity of type {EntityType}",
                typeof(TEntity).Name);

            throw SmartAttendanceException.InternalServerError();
        }
    }

    public async Task LoadCollectionAsync<TProperty>(
        IEnumerable<TEntity> entities,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty,
        CancellationToken cancellationToken = default)
        where TProperty : class
    {
        foreach (var entity in entities)
        {
            await DbContext.Entry(entity).Collection(collectionProperty).LoadAsync(cancellationToken);
        }
    }

    public async Task LoadReferenceAsync<TProperty>(
        IEnumerable<TEntity> entities,
        Expression<Func<TEntity, TProperty>> referenceProperty,
        CancellationToken cancellationToken = default)
        where TProperty : class
    {
        foreach (var entity in entities)
        {
            await DbContext.Entry(entity).Reference(referenceProperty).LoadAsync(cancellationToken);
        }
    }
}