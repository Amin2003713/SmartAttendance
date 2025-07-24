using Shifty.Common.General.Enums;
using Shifty.Domain.Defaults;

namespace Shifty.Persistence.Services.Seeder;

public class Seeder : IScopedDependency
{
    public async Task SeedAsync(ShiftyDbContext dbContext, CancellationToken cancellationToken)
    {
        try
        {
            await SeedRoles(dbContext, cancellationToken);
            await SeedDefaultSettings(dbContext, cancellationToken);


            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw IpaException.InternalServerError();
        }
    }


    private async Task SeedDefaultSettings(ShiftyDbContext dbContext, CancellationToken cancellationToken)
    {
        var setting = TenantDefaultValue.Setting();


        if (await dbContext.Set<Setting>().AnyAsync(a => a.Id == setting.Id, cancellationToken))
            return;


        dbContext.Add(setting);
    }


    private async Task SeedRoles(ShiftyDbContext dbContext, CancellationToken cancellationToken)
    {
        foreach (var role in Enum.GetValues<Roles>())
        {
            if (await dbContext.Roles.AnyAsync(r => r.Name == role.ToString(), cancellationToken))
                continue;

            var newRole = new Role
            {
                Name = role.ToString(),
                Id = Guid.CreateVersion7(),
                Description = role.ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                IsActive = true,
                NormalizedName = role.ToString().ToUpper()
            };

            dbContext.Roles.Add(newRole);
        }
    }
}