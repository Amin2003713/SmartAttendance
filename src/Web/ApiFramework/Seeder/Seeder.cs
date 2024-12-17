using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Domain.Enums;
using Shifty.Domain.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shifty.ApiFramework.Seeder;

public static class Seeder
{


    public static async void SeedRoles(this IServiceProvider services)
    {
        try
        {
            using var scope       = services.CreateScope();
            var       roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            foreach (var role in Enum.GetValues<UserRoles>())
            {
                var roleName  = role.ToString();
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (roleExist)
                    continue;

                var addRoleResult = await roleManager.CreateAsync(new Role
                {
                    Name = roleName,
                    Description = $"{roleName} role"
                });

                if (addRoleResult.Succeeded)
                    continue;

                // Handle error, logging or throw exception
                Console.WriteLine($"Error creating role {roleName}: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
            }
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }
}