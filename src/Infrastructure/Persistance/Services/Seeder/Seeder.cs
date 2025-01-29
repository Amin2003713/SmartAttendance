using Microsoft.EntityFrameworkCore;
using Shifty.Domain.Enums;
using Shifty.Persistence.Db;
using System;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Domain.Features.Users;

namespace Shifty.Persistence.Services.Seeder
{
    public class Seeder(IServiceProvider provider)
    {
        public async Task Seed(AppDbContext dbContext , CancellationToken cancellationToken)
        {
            await SeedRoles(dbContext , cancellationToken);
        }

        public async Task SeedRoles(AppDbContext dbContext , CancellationToken cancellationToken)
        {
            foreach (var role in Enum.GetValues<UserRoles>())
            {
                if (await dbContext.Roles.AnyAsync(r => r.Name == role.ToString() , cancellationToken))
                    continue;

                var newRole = new Role
                {
                    Name     = role.ToString() , Id  = Guid.CreateVersion7() , Description = role.ToString() , ConcurrencyStamp = Guid.NewGuid().ToString() ,
                    IsActive = true , NormalizedName = role.ToString().ToUpper() };

                dbContext.Roles.Add(newRole);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}