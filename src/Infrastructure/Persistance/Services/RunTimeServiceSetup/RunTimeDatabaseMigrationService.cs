using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.Utilities.EnumHelpers;
using SmartAttendance.Common.Utilities.InjectionHelpers;

namespace SmartAttendance.Persistence.Services.RunTimeServiceSetup;

public class RunTimeDatabaseMigrationService(
    IServiceProvider services,
    Seeder.Seeder seeder,
    IPasswordHasher<User> passwordHasher,
    SmartAttendanceTenantDbContext tenantDbContext,
    UserManager<User> userManager
) : IScopedDependency
{
    public virtual async Task<string> MigrateTenantDatabasesAsync(
        UniversityTenantInfo tenantInfo,
        string passWord,
        UniversityAdmin adminUser,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tenantInfo.GetConnectionString());

        using var scopeApplication = services.CreateScope();
        var       dbContext        = scopeApplication.ServiceProvider.GetRequiredService<SmartAttendanceDbContext>();
        dbContext.Database.SetConnectionString(tenantInfo.GetConnectionString());

        try
        {
            // Apply pending migrations
            if (!await dbContext.Database.CanConnectAsync(cancellationToken) ||
                (await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                await dbContext.Database.MigrateAsync(cancellationToken);
            }

            // Seed database
            await seeder.SeedAsync(dbContext, cancellationToken);

            // Map admin user
            var user = adminUser.Adapt<User>();
            user.SetPasswordHash(passwordHasher.HashPassword(user, passWord));
            dbContext.Users.Add(user);

            // Add user to tenant
            AddUserToTenant(tenantInfo, user);
            await tenantDbContext.SaveChangesAsync(cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            // Grant Admin Role
            await EnsureAdminRoleExistsAsync(dbContext, user, cancellationToken);

            // Generate 2FA code
            return await userManager.GenerateTwoFactorTokenAsync(user, ApplicationConstant.Identity.CodeGenerator);
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($@"Migration Service Error: {e}");
            Console.ResetColor();
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    private void AddUserToTenant(UniversityTenantInfo tenantInfo, User user)
    {
        var universityUser = user.Adapt<UniversityUser>();
        universityUser.Id = Guid.CreateVersion7(DateTimeOffset.Now);
        universityUser.UniversityTenantInfoId = tenantInfo.Id;
        tenantDbContext.UniversityUsers.Add(universityUser);
    }

    private async Task EnsureAdminRoleExistsAsync(SmartAttendanceDbContext dbContext, User user, CancellationToken cancellationToken)
    {
        var adminRoleName = Roles.Admin.GetEnglishName().ToLower();

        // Ensure role exists
        if (!await dbContext.Roles.AnyAsync(r => r.Name == adminRoleName, cancellationToken))
        {
            var role = new IdentityRole<Guid>
            {
                Name = adminRoleName,
                NormalizedName = adminRoleName.ToUpper()
            };

            dbContext.Roles.Add(role);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var adminRole = await dbContext.Roles.FirstAsync(r => r.Name == adminRoleName , cancellationToken);

        // Assign role to user
        if (!await dbContext.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == adminRole.Id, cancellationToken))
        {
            dbContext.UserRoles.Add(new IdentityUserRole<Guid>
            {
                RoleId = adminRole.Id,
                UserId = user.Id
            });

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}