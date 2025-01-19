using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shifty.Common.Utilities;
using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.Db
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User , Role , Guid>(options) , IAppDbContext
    {
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
    }
}