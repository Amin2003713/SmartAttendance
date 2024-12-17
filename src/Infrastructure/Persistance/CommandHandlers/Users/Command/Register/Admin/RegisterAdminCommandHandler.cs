using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shifty.Application.Users.Command.CreateUser.Admin;
using Shifty.Common.Exceptions;
using Shifty.Domain.Enums;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Db;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Users.Command.Register.Admin
{
    public class RegisterAdminCommandHandler
        (TenantDbContext context , IPasswordHasher<TenantAdmin> passwordHasher) : IRequestHandler<RegisterAdminCommand, bool>
    {
        public async Task<bool> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                    throw new InvalidNullInputException(nameof(request));

                var user = request.Adapt<TenantAdmin>();
                user.SetUserName();

                var createUserResult = await CreateUserAsync(user, request.MobileNumber);

               

                var ten = new ShiftyTenantInfo()
                {
                    Company = request.CompanyInfo.Adapt<Company>(), Name = request.CompanyInfo.Name
                    , Identifier = request.CompanyInfo.ApplicationAccessDomain
                    , User = user,
                };

                context.TenantInfo.Add(ten);
                await context.SaveChangesAsync(cancellationToken);

                return createUserResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> CreateUserAsync(TenantAdmin user, string passwordOrMobileNumber)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null");

            if (string.IsNullOrWhiteSpace(passwordOrMobileNumber))
                throw new ArgumentException("Password cannot be null or empty.", nameof(passwordOrMobileNumber));

            // Check for duplicate users
            bool userExists = await context.Users.AnyAsync(u => u.UserName == user.UserName);

            if (userExists)
                throw new InvalidOperationException("A user with the same username already exists.");

            // Hash the password
            user.PasswordHash = passwordHasher.HashPassword(user, passwordOrMobileNumber);

            // Set additional user properties (e.g., CreatedAt, SecurityStamp, etc.)
            user.SecurityStamp = Guid.NewGuid().ToString();

            // Add the user to the database
            context.Users.Add(user);
            var result = await context.SaveChangesAsync();

            return result > 0;
        }
    }
}
