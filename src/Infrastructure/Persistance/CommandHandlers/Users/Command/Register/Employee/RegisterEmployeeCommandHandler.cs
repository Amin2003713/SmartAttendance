using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shifty.Application.Users.Command.CreateUser.Employee;
using Shifty.Common;
using Shifty.Common.Exceptions;
using Shifty.Domain.Users;
using Shifty.Domain.Users.Exceptions;
using Shifty.Persistence.Services.Seeder;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Users.Command.Register.Employee;

public class RegisterEmployeeCommandHandler(UserManager<User> userManager , Seeder seeder) : IRequestHandler<RegisterEmployeeCommand, bool>
{
    public async Task<bool> Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new InvalidNullInputException(nameof(request));

        var user = request.Adapt<User>();

        user.SetUserName();
        var createUserResult = await userManager.CreateAsync(user, request.PhoneNumber);

        foreach (var role in request.RolesList)
        {
            var result = await userManager.AddToRoleAsync(user, role);

            if(!result.Succeeded)
                throw new ShiftyException(ApiResultStatusCode.ServerError , UserErrors.There_was_An_Error_While_Adding_User_To_Roles ,  result.Errors);
        }
        

        return createUserResult.Succeeded;
    }
}