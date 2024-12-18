using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Application.Users.Command.CreateUser.Admin;
using Shifty.Common.Exceptions;
using Shifty.Domain.Enums;
using Shifty.Domain.Interfaces.Users;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Services.Seeder;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Users.Command.Register.Admin
{
    public class RegisterAdminCommandHandler(
          ITenantAdminRepository userManager 
         , Seeder seeder
        ) : IRequestHandler<RegisterAdminCommand, bool>
    {
        public async Task<bool> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                    throw new InvalidNullInputException(nameof(request));

                var user = request.Adapt<TenantAdmin>();

                var createUserResult = await userManager.CreateAsync(user, request.MobileNumber , cancellationToken);

                return createUserResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


    }
}