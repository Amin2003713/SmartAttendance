using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shifty.Common.Utilities;
using Shifty.Domain.Common.BaseClasses;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shifty.Domain.Features.Users;

namespace Shifty.Persistence.Db
{
    public class AppDbContext : IdentityDbContext<User , Role , Guid> , IAppDbContext
    {
        private readonly Guid? _currentUserId;

        public AppDbContext(DbContextOptions<AppDbContext> options , Guid? currentUserId) : base(options)
        {
            _currentUserId = currentUserId;
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public async Task<int> ExecuteSqlRawAsync(string query , CancellationToken cancellationToken)
        {
            var result = await base.Database.ExecuteSqlRawAsync(query , cancellationToken);
            return result;
        }

        public async Task<int> ExecuteSqlRawAsync(string query)
        {
            return await ExecuteSqlRawAsync(query , CancellationToken.None);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntity).Assembly;

            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntity).Assembly);
            modelBuilder.AddPluralizingTableNameConvention();
        }



            public override int SaveChanges()
            {
                ApplyEntityRules();
                return base.SaveChanges();
            }

            public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                ApplyEntityRules();
                return await base.SaveChangesAsync(cancellationToken);
            }

            private void ApplyEntityRules()
            {
                var entries = ChangeTracker.Entries<BaseEntity>();

                foreach (var entry in entries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedAt =   DateTime.UtcNow;
                            entry.Entity.CreatedBy ??= (_currentUserId ?? null!);
                            entry.Entity.IsActive  =   true;
                            break;

                        case EntityState.Modified:
                            entry.Entity.ModifiedAt =   DateTime.UtcNow;
                            entry.Entity.ModifiedBy ??= (_currentUserId ?? null!);
                            break;

                        case EntityState.Deleted:
                            entry.Entity.DeletedAt = DateTime.UtcNow;
                            entry.Entity.DeletedBy ??= (_currentUserId ?? null!);
                            entry.Entity.IsActive  = false;
                            entry.State            = EntityState.Modified; 
                            break;
                    }
                }
            }
        }

    }
