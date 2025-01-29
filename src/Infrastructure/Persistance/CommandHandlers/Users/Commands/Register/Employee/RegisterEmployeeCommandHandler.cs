using System;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shifty.Application.Users.Command.CreateUser.Employee;
using Shifty.Common.Exceptions;
using Shifty.Common.Utilities;
using Shifty.Domain.Features.Users;

namespace Shifty.Persistence.CommandHandlers.Users.Commands.Register.Employee
{
    public class RegisterEmployeeCommandHandler(UserManager<User> userManager , ILogger<RegisterEmployeeCommandHandler> logger) : IRequestHandler<RegisterEmployeeCommand , bool>
    {
        public async Task<bool> Handle(RegisterEmployeeCommand request , CancellationToken cancellationToken)
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            try
            {
                var user = request.Adapt<User>();

                user.SetUserName();
                var createUserResult = await userManager.CreateAsync(user , PasswordGenerator.GeneratePassword());

                foreach (var role in request.RolesList)
                {
                    var result = await userManager.AddToRoleAsync(user , role);

                    if (result.Succeeded)
                        continue;

                    foreach (var error in result.Errors)
                        logger.LogError(error.Code , error.Description);
                    throw ShiftyException.InternalServerError();
                }


                return createUserResult.Succeeded;
            }
            catch (ShiftyException e)
            {
                logger.LogError(e.Source , e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}