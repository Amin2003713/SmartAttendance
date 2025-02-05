using Microsoft.EntityFrameworkCore;
using Shifty.Domain.Enums;
using Shifty.Persistence.Db;
using System;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Common;
using Shifty.Domain.Defaults;
using Shifty.Domain.Features.Divisions;
using Shifty.Domain.Features.Setting;
using Shifty.Domain.Features.Shifts;
using Shifty.Domain.Features.Users;

namespace Shifty.Persistence.Services.Seeder
{
    public class Seeder() : IScopedDependency
    {
        public async Task Seed(AppDbContext dbContext , CancellationToken cancellationToken)
        {
            await SeedRoles(dbContext , cancellationToken);
            await SeedShifts(dbContext , cancellationToken);
            await SeedDivisions(dbContext , cancellationToken);
            await SeedDefaultSettings(dbContext , cancellationToken);




            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task SeedDefaultSettings(AppDbContext dbContext , CancellationToken cancellationToken)
        {
            var setting = Defaults.GetSettings();

            if (await dbContext.Set<Setting>().AnyAsync(cancellationToken: cancellationToken))
                return;

            dbContext.Add(setting);
        }

        private async Task SeedShifts(AppDbContext dbContext , CancellationToken cancellationToken)
        {
            var shifts = Defaults.GetDefaultShifts();

            if(await dbContext.Set<Shift>().AnyAsync(cancellationToken: cancellationToken))
                return;

            foreach (var shift in shifts)
                dbContext.Add(shift);

        }

        private async Task SeedDivisions(AppDbContext dbContext , CancellationToken cancellationToken)
        {
            if (await dbContext.Set<Division>().AnyAsync(cancellationToken: cancellationToken))
                return;
            var division = Defaults.GetDivisions();
            dbContext.Add(division);
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
            }
        }
    }
}