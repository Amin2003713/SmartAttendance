using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Defaults;

namespace Shifty.Persistence.Services.RunTimeServiceSetup;

public class RunTimeDatabaseMigrationService(
    IServiceProvider services,
    Seeder.Seeder seeder,
    IPasswordHasher<User> passwordHasher,
    ShiftyTenantDbContext tenantDbContext,
    UserManager<User> userManager
) : IScopedDependency
{
    public virtual async Task<string> MigrateTenantDatabasesAsync(
        ShiftyTenantInfo tenantInfo,
        string passWord,
        TenantAdmin adminUser,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tenantInfo.GetConnectionString());

        using var scopeTenant = services.CreateScope();

        using var scopeApplication = services.CreateScope();
        var       dbContext        = scopeApplication.ServiceProvider.GetService<ShiftyDbContext>();
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
                throw ShiftyException.InternalServerError("migiration appliyeing");
            }


            try
            {
                await seeder.SeedAsync(dbContext, cancellationToken);


                var user = adminUser.Adapt<User>();
                user.IsLeader = true;

                user.SetPasswordHash(passwordHasher.HashPassword(user, passWord));

                dbContext.Users.Add(user);

                var payment = TenantDefaultValue.DemoPayment(user, tenantInfo);
                tenantDbContext.Payments.Add(payment);

                // var activeService = TenantDefaultValue.CreateActiveService(payment);
                // await tenantDbContext.Set<ActiveService>().AddRangeAsync(activeService, cancellationToken);


                AddUserToTenant(tenantInfo, user);
                await tenantDbContext.SaveChangesAsync(cancellationToken);


                await dbContext.SaveChangesAsync(cancellationToken);
                return await GenerateCode(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw ShiftyException.InternalServerError();
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
            throw ShiftyException.InternalServerError();
        }
    }

    private void AddUserToTenant(ShiftyTenantInfo tenantInfo, User user)
    {
        var tenantUser = user.Adapt<TenantUser>();
        tenantUser.Id = Guid.CreateVersion7(DateTimeOffset.Now);
        tenantUser.ShiftyTenantInfoId = tenantInfo.Id;
        tenantDbContext.TenantUsers.Add(tenantUser);
    }

    private async Task<string> GenerateCode(User user)
    {
        return await userManager.GenerateTwoFactorTokenAsync(user, ApplicationConstant.Identity.CodeGenerator);
    }
}