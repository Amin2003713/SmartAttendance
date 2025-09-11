using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Defaults;

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
        SmartAttendanceTenantInfo tenantInfo,
        string passWord,
        TenantAdmin adminUser,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tenantInfo.GetConnectionString());

        using var scopeTenant = services.CreateScope();

        using var scopeApplication = services.CreateScope();
        var       dbContext        = scopeApplication.ServiceProvider.GetService<SmartAttendanceDbContext>();
        dbContext!.Database.SetConnectionString(tenantInfo.GetConnectionString());

        try
        {
            try
            {
                if (!await dbContext.Database.CanConnectAsync(cancellationToken))
                    await dbContext.Database.MigrateAsync(cancellationToken);


                if ((await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                    await dbContext.Database.MigrateAsync(cancellationToken); // apply migrations on baseDbContext
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw SmartAttendanceException.InternalServerError("migiration appliyeing");
            }


            try
            {
                await seeder.SeedAsync(dbContext, cancellationToken);


                var user = adminUser.Adapt<User>();
                user.IsLeader = true;

                user.SetPasswordHash(passwordHasher.HashPassword(user, passWord));

                dbContext.Users.Add(user);

                AddUserToTenant(tenantInfo, user);
                await tenantDbContext.SaveChangesAsync(cancellationToken);


                await dbContext.SaveChangesAsync(cancellationToken);
                return await GenerateCode(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw SmartAttendanceException.InternalServerError();
            }
            finally
            {
                var adminRoles = await dbContext.Roles.ToListAsync(cancellationToken);
                if (adminRoles != null)
                    dbContext.UserRoles.AddRange(adminRoles.Select(a => new UserRoles
                    {
                        RoleId = a.Id,
                        UserId = adminUser.Id
                    }));

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($@"Migration Managers Error: {e.Message}");
            Console.ResetColor();
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    private void AddUserToTenant(SmartAttendanceTenantInfo tenantInfo, User user)
    {
        var tenantUser = user.Adapt<TenantUser>();
        tenantUser.Id = Guid.CreateVersion7(DateTimeOffset.Now);
        tenantUser.SmartAttendanceTenantInfoId = tenantInfo.Id;
        tenantDbContext.TenantUsers.Add(tenantUser);
    }

    private async Task<string> GenerateCode(User user)
    {
        return await userManager.GenerateTwoFactorTokenAsync(user, ApplicationConstant.Identity.CodeGenerator);
    }
}