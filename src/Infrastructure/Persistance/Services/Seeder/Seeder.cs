using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Defaults;

namespace SmartAttendance.Persistence.Services.Seeder;

public class Seeder : IScopedDependency,
    IGenericSeeder<SmartAttendanceDbContext>
{
    public async Task SeedAsync(SmartAttendanceDbContext dbContext, CancellationToken cancellationToken)
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
            throw SmartAttendanceException.InternalServerError();
        }
    }


    private async Task SeedDefaultSettings(SmartAttendanceDbContext dbContext, CancellationToken cancellationToken)
    {
        var setting = TenantDefaultValue.Setting();


        if (await dbContext.Set<Setting>().AnyAsync(a => a.Id == setting.Id, cancellationToken))
            return;


        dbContext.Add(setting);
    }


    private async Task SeedRoles(SmartAttendanceDbContext dbContext, CancellationToken cancellationToken)
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