using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Common.General;
using Shifty.Common.Utilities;
using Shifty.Domain.Enums;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Db;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.Services.MigrationManagers
{
    public class RunTimeDatabaseMigrationService(IServiceProvider services, IConfiguration configuration , Seeder.Seeder seeder , IPasswordHasher<User> passwordHasher)
    {
        public async Task<bool> MigrateTenantDatabasesAsync(string connectionString, TenantAdmin adminUser , CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(connectionString);

                using var scopeTenant     = services.CreateScope();

                    using var scopeApplication = services.CreateScope();
                    var       dbContext        = scopeApplication.ServiceProvider.GetService<AppDbContext>();
                    dbContext.Database.SetConnectionString(connectionString);

                    // await dbContext.Database.EnsureCreatedAsync();

                    if (!(await dbContext.Database.GetPendingMigrationsAsync(cancellationToken: cancellationToken)).Any())
                        return true;


                    try
                    {
                        await dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);


                        await seeder.Seed(dbContext, cancellationToken);


                        var user = adminUser.Adapt<User>();

                        user.SetPasswordHash(passwordHasher.HashPassword(user, PasswordGenerator.GeneratePassword())) ;

                        dbContext.Users.Add(user);

                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return false;
                    }
                    finally
                    {
                        var adminRoles =await dbContext.Roles.FirstOrDefaultAsync(a=>a.Name == UserRoles.Admin.ToString(), cancellationToken: cancellationToken);
                        if (adminRoles != null)
                        {
                            dbContext.UserRoles.Add(new IdentityUserRole<Guid>(){RoleId = adminRoles.Id, UserId = adminUser.Id});
                            await dbContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                    
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"MigrationManagers Error: {e.Message}");
                Console.ResetColor();
                return false;
            }

            return true;
        }
    }
}