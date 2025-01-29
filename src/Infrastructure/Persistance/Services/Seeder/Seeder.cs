using Microsoft.EntityFrameworkCore;
using Shifty.Domain.Enums;
using Shifty.Persistence.Db;
using System;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Defaults;
using Shifty.Domain.Features.Users;

namespace Shifty.Persistence.Services.Seeder
{
    public class Seeder()
    {
        public async Task Seed(AppDbContext dbContext , CancellationToken cancellationToken)
        {
            await SeedRoles(dbContext , cancellationToken);
            await SeedShifts(dbContext , cancellationToken);
            await SeedDivisions(dbContext , cancellationToken);
        }

        private async Task SeedShifts(AppDbContext dbContext , CancellationToken cancellationToken)
        {
            var shifts = Defaults.GetDefaultShifts();

            foreach (var shift in shifts)
                dbContext.Add(shift);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task SeedDivisions(AppDbContext dbContext , CancellationToken cancellationToken)
        {
           //
        }

        private async Task SeedRoles(AppDbContext dbContext , CancellationToken cancellationToken)
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