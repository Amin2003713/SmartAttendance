using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Common.Utilities;
using Shifty.Domain.Constants;
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
    public class RunTimeDatabaseMigrationService(
        IServiceProvider services ,
        IConfiguration configuration ,
        Seeder.Seeder seeder ,
        IPasswordHasher<User> passwordHasher ,
        UserManager<User> userManager)
    {
        public IConfiguration Configuration { get; } = configuration;

        public async Task<string> MigrateTenantDatabasesAsync(string connectionString , TenantAdmin adminUser , CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(connectionString);


                using var scopeTenant = services.CreateScope();

                using var scopeApplication = services.CreateScope();
                var       dbContext        = scopeApplication.ServiceProvider.GetService<AppDbContext>();
                dbContext.Database.SetConnectionString(connectionString);

                // await dbContext.Database.EnsureCreatedAsync();

                if (!(await dbContext.Database.GetPendingMigrationsAsync(cancellationToken: cancellationToken)).Any())
                    return null;


                try
                {
                    await dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);


                    await seeder.Seed(dbContext , cancellationToken);


                    var user = adminUser.Adapt<User>();

                    user.SetPasswordHash(passwordHasher.HashPassword(user , PasswordGenerator.GeneratePassword()));

                    dbContext.Users.Add(user);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    return await GenerateCode(user);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
                finally
                {
                    var adminRoles = await dbContext.Roles.FirstOrDefaultAsync(a => a.Name == UserRoles.Admin.ToString() , cancellationToken);
                    if (adminRoles == null)

                        dbContext.UserRoles.Add(new IdentityUserRole<Guid>
                        {
                            RoleId = adminRoles.Id , UserId = adminUser.Id ,
                        });
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"MigrationManagers Error: {e.Message}");
                Console.ResetColor();
                return null;
            }
        }

        private async Task<string> GenerateCode(User user)
        {
            return await userManager.GenerateTwoFactorTokenAsync(user , ApplicationConstant.Identity.CodeGenerator);
        }
    }
}