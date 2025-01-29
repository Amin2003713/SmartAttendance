using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shifty.Common; // Added for logging
using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Shifty.Common.Exceptions;

namespace Shifty.Persistence.Repositories.Common
{
    public class Repository<TEntity, TReadDb> : IRepository<TEntity> where TEntity : class, IEntity where TReadDb : DbContext
    {
        protected readonly TReadDb DbContext;
        private readonly ILogger<Repository<TEntity, TReadDb>> _logger; 

        public Repository(TReadDb dbContext, ILogger<Repository<TEntity, TReadDb>> logger) // Constructor updated
        {
            DbContext = dbContext;
            Entities = DbContext.Set<TEntity>();
            _logger = logger;
        }

        public DbSet<TEntity> Entities { get; }
        public virtual IQueryable<TEntity> Table => Entities;
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        #region Async Methods

        public virtual async ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
        {
            try
            {
                _logger.LogInformation("Fetching entity of type {EntityType} ", typeof(TEntity).Name);
                var entity = await Entities.FindAsync(ids, cancellationToken).ConfigureAwait(false);
                if (entity == null)
                {
                    _logger.LogWarning("Entity of type {EntityType}  not found", typeof(TEntity).Name);
                }
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching entity of type {EntityType} ", typeof(TEntity).Name);
                throw ShiftyException.NotFound();
            }
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Adding entity of type {EntityType}", typeof(TEntity).Name);
                await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
                if (saveNow)
                {
                    await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation("Entity of type {EntityType} added successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity , bool>> predations , CancellationToken cancellationToken = default)
            => await TableNoTracking.AnyAsync(predations , cancellationToken);

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Adding range of entities of type {EntityType}", typeof(TEntity).Name);
                await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
                if (saveNow)
                {
                    await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation("Entities of type {EntityType} added successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , "Error adding entities of type {EntityType} {ex}" , typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Updating entity of type {EntityType}", typeof(TEntity).Name);
                Entities.Update(entity);
                if (saveNow)
                {
                    await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation("Entity of type {EntityType} updated successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity of type {EntityType} {ex}" , typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Updating range of entities of type {EntityType}", typeof(TEntity).Name);
                Entities.UpdateRange(entities);
                if (saveNow)
                {
                    await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation("Entities of type {EntityType} updated successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating range of entities of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching single entity of type {EntityType} with predicate", typeof(TEntity).Name);
                return Entities.SingleOrDefaultAsync(predicate, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching single entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                return null!;
            }
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Deleting entity of type {EntityType}", typeof(TEntity).Name);
                Entities.Remove(entity);
                if (saveNow)
                {
                    await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation("Entity of type {EntityType} deleted successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Deleting range of entities of type {EntityType}", typeof(TEntity).Name);
                Entities.RemoveRange(entities);
                if (saveNow)
                {
                    await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation("Entities of type {EntityType} deleted successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting range of entities of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        #endregion

        #region Sync Methods

        public virtual TEntity GetById(params object[] ids)
        {
            try
            {
                _logger.LogInformation("Fetching entity of type {EntityType} ", typeof(TEntity).Name);
                var entity = Entities.Find(ids);

                if (entity != null)
                    return entity;

                _logger.LogWarning("Entity of type {EntityType}  not found", typeof(TEntity).Name);
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching entity of type {EntityType} ", typeof(TEntity).Name);
                return null!;
              
            }
        }

        public virtual void Add(TEntity entity, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Adding entity of type {EntityType}", typeof(TEntity).Name);
                Entities.Add(entity);
                if (saveNow)
                {
                    DbContext.SaveChanges();
                    _logger.LogInformation("Entity of type {EntityType} added successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual void AddRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Adding range of entities of type {EntityType}", typeof(TEntity).Name);
                Entities.AddRange(entities);
                if (saveNow)
                {
                    DbContext.SaveChanges();
                    _logger.LogInformation("Entities of type {EntityType} added successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding range of entities of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual void Update(TEntity entity, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Updating entity of type {EntityType}", typeof(TEntity).Name);
                Entities.Update(entity);
                if (saveNow)
                {
                    DbContext.SaveChanges();
                    _logger.LogInformation("Entity of type {EntityType} updated successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Updating range of entities of type {EntityType}", typeof(TEntity).Name);
                Entities.UpdateRange(entities);
                if (saveNow)
                {
                    DbContext.SaveChanges();
                    _logger.LogInformation("Entities of type {EntityType} updated successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating range of entities of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual void Delete(TEntity entity, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Deleting entity of type {EntityType}", typeof(TEntity).Name);
                Entities.Remove(entity);
                if (saveNow)
                {
                    DbContext.SaveChanges();
                    _logger.LogInformation("Entity of type {EntityType} deleted successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            try
            {
                _logger.LogInformation("Deleting range of entities of type {EntityType}", typeof(TEntity).Name);
                Entities.RemoveRange(entities);
                if (saveNow)
                {
                    DbContext.SaveChanges();
                    _logger.LogInformation("Entities of type {EntityType} deleted successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting range of entities of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        #endregion

        #region Attach & Detach

        public virtual void Detach(TEntity entity)
        {
            try
            {
                _logger.LogInformation("Detaching entity of type {EntityType}", typeof(TEntity).Name);
                var entry = DbContext.Entry(entity);
                if (entry != null)
                {
                    entry.State = EntityState.Detached;
                    _logger.LogInformation("Entity of type {EntityType} detached successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detaching entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual void Attach(TEntity entity)
        {
            try
            {
                _logger.LogInformation("Attaching entity of type {EntityType}", typeof(TEntity).Name);
                if (DbContext.Entry(entity).State == EntityState.Detached)
                {
                    Entities.Attach(entity);
                    _logger.LogInformation("Entity of type {EntityType} attached successfully", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attaching entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        #endregion

        #region Explicit Loading

        public virtual async Task LoadCollectionAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty,
            CancellationToken cancellationToken) where TProperty : class
        {
            try
            {
                _logger.LogInformation("Loading collection property for entity of type {EntityType}", typeof(TEntity).Name);
                Attach(entity);

                var collection = DbContext.Entry(entity).Collection(collectionProperty);
                if (!collection.IsLoaded)
                {
                    await collection.LoadAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation("Collection property loaded for entity of type {EntityType}", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading collection property for entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual void LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
            where TProperty : class
        {
            try
            {
                _logger.LogInformation("Loading collection property for entity of type {EntityType}", typeof(TEntity).Name);
                Attach(entity);

                var collection = DbContext.Entry(entity).Collection(collectionProperty);
                if (!collection.IsLoaded)
                {
                    collection.Load();
                    _logger.LogInformation("Collection property loaded for entity of type {EntityType}", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading collection property for entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual async Task LoadReferenceAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> referenceProperty,
            CancellationToken cancellationToken) where TProperty : class
        {
            try
            {
                _logger.LogInformation("Loading reference property for entity of type {EntityType}", typeof(TEntity).Name);
                Attach(entity);

                var reference = DbContext.Entry(entity).Reference(referenceProperty);
                if (!reference.IsLoaded)
                {
                    await reference.LoadAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogInformation("Reference property loaded for entity of type {EntityType}", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading reference property for entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        public virtual void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty) where TProperty : class
        {
            try
            {
                _logger.LogInformation("Loading reference property for entity of type {EntityType}", typeof(TEntity).Name);
                Attach(entity);

                var reference = DbContext.Entry(entity).Reference(referenceProperty);
                if (!reference.IsLoaded)
                {
                    reference.Load();
                    _logger.LogInformation("Reference property loaded for entity of type {EntityType}", typeof(TEntity).Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading reference property for entity of type {EntityType} {ex}", typeof(TEntity).Name , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        #endregion
    }
}
