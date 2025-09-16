using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Persistence.Repositories.Common;

public class CommandRepository<TEntity>(
    WriteOnlyDbContext                  dbContext,
    ILogger<CommandRepository<TEntity>> logger
)
    : RepositoryBase<TEntity, WriteOnlyDbContext>(dbContext, logger),
        ICommandRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly WriteOnlyDbContext _dbContext = dbContext;

    public void Add(TEntity entity, bool saveNow = true)
    {
        try
        {
            DbContext.ChangeTracker.Clear();
            Logger.LogInformation("Adding entity of type {EntityType}", typeof(TEntity).Name);
            Entities.Add(entity);

            if (saveNow)
            {
                _dbContext.SaveChanges();
                Logger.LogInformation("Entity of type {EntityType} added successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error adding entity of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public void AddRange(IEnumerable<TEntity> entities, bool saveNow = true)
    {
        try
        {
            DbContext.ChangeTracker.Clear();
            Logger.LogInformation("Adding range of entities of type {EntityType}", typeof(TEntity).Name);
            Entities.AddRange(entities);

            if (saveNow)
            {
                _dbContext.SaveChanges();
                Logger.LogInformation("Entities of type {EntityType} added successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error adding range of entities of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public void Update(TEntity entity, bool saveNow = true)
    {
        try
        {
            DbContext.ChangeTracker.Clear();
            Logger.LogInformation("Updating entity of type {EntityType}", typeof(TEntity).Name);
            Entities.Update(entity);

            if (saveNow)
            {
                _dbContext.SaveChanges();
                Logger.LogInformation("Entity of type {EntityType} updated successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error updating entity of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public void UpdateRange(IEnumerable<TEntity> entities, bool saveNow = true)
    {
        try
        {
            DbContext.ChangeTracker.Clear();
            Logger.LogInformation("Updating range of entities of type {EntityType}", typeof(TEntity).Name);
            Entities.UpdateRange(entities);

            if (saveNow)
            {
                _dbContext.SaveChanges();
                Logger.LogInformation("Entities of type {EntityType} updated successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error updating range of entities of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public void Delete(TEntity entity, bool saveNow = true)
    {
        try
        {
            DbContext.ChangeTracker.Clear();
            Logger.LogInformation("Deleting entity of type {EntityType}", typeof(TEntity).Name);
            Entities.Remove(entity);

            if (saveNow)
            {
                _dbContext.SaveChanges();
                Logger.LogInformation("Entity of type {EntityType} deleted successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting entity of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true)
    {
        try
        {
            Logger.LogInformation("Deleting range of entities of type {EntityType}", typeof(TEntity).Name);
            Entities.RemoveRange(entities);

            if (saveNow)
            {
                _dbContext.SaveChanges();
                Logger.LogInformation("Entities of type {EntityType} deleted successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting range of entities of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public void Attach(TEntity entity)
    {
        try
        {
            Logger.LogInformation("Attaching entity of type {EntityType}", typeof(TEntity).Name);

            if (_dbContext.Entry(entity).State ==
                EntityState.Detached)
            {
                Entities.Attach(entity);
                Logger.LogInformation("Entity of type {EntityType} attached successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error attaching entity of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public void Detach(TEntity entity)
    {
        try
        {
            Logger.LogInformation("Detaching entity of type {EntityType}", typeof(TEntity).Name);
            var entry = _dbContext.Entry(entity);

            if (entry != null)
            {
                entry.State = EntityState.Detached;
                Logger.LogInformation("Entity of type {EntityType} detached successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error detaching entity of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
    {
        try
        {
            DbContext.ChangeTracker.Clear();
            Logger.LogInformation("Adding entity of type {EntityType}", typeof(TEntity).Name);
            DbContext.Add(entity);

            if (saveNow)
            {
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                Logger.LogInformation("Entity of type {EntityType} added successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error adding entity of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public async Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken    cancellationToken,
        bool                 saveNow = true)
    {
        try
        {
            DbContext.ChangeTracker.Clear();
            Logger.LogInformation("Adding range of entities of type {EntityType}", typeof(TEntity).Name);
            await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);

            if (saveNow)
            {
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                Logger.LogInformation("Entities of type {EntityType} added successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error adding range of entities of type {EntityType} , {ex}", typeof(TEntity).Name, ex);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
    {
        try
        {
            DbContext.ChangeTracker.Clear();
            Logger.LogInformation("Updating entity of type {EntityType}", typeof(TEntity).Name);
            Entities.Update(entity);

            if (saveNow)
            {
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                Logger.LogInformation("Entity of type {EntityType} updated successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error updating entity of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public async Task UpdateRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken    cancellationToken,
        bool                 saveNow = true)
    {
        try
        {
            DbContext.ChangeTracker.Clear();
            Logger.LogInformation("Updating range of entities of type {EntityType}", typeof(TEntity).Name);
            Entities.UpdateRange(entities);

            if (saveNow)
            {
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                Logger.LogInformation("Entities of type {EntityType} updated successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error updating range of entities of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
    {
        try
        {
            DbContext.ChangeTracker.Clear();
            Logger.LogInformation("Deleting entity of type {EntityType}", typeof(TEntity).Name);
            Entities.Remove(entity);

            if (saveNow)
            {
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                Logger.LogInformation("Entity of type {EntityType} deleted successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting entity of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    public async Task DeleteAsync(
        Expression<Func<TEntity, bool>> prediction,
        CancellationToken               cancellationToken,
        bool                            saveNow = true)
    {
        try
        {
            var item = await Entities.FirstOrDefaultAsync(prediction, cancellationToken);

            if (item == null)
                SmartAttendanceException.NotFound();

            await DeleteAsync(item, cancellationToken, saveNow);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken    cancellationToken,
        bool                 saveNow = true)
    {
        try
        {
            Logger.LogInformation("Deleting range of entities of type {EntityType}", typeof(TEntity).Name);
            Entities.RemoveRange(entities);

            if (saveNow)
            {
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                Logger.LogInformation("Entities of type {EntityType} deleted successfully", typeof(TEntity).Name);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting range of entities of type {EntityType}", typeof(TEntity).Name);
            throw SmartAttendanceException.InternalServerError();
        }
    }
}