using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shifty.Common.Exceptions;
using Shifty.Domain.Entities.Users;
using Shifty.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Application.Users.Command.CreateUser.Admin
{
    public class RegisterAdminCommandHandler
        (UserManager<User> userManager) : IRequestHandler<RegisterAdminCommand, bool>
    {
        public async Task<bool> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var user=  request.Adapt<User>();
            user.SetUserName();

            var createUserResult = await userManager.CreateAsync(user, request.MobileNumber);

            var assignRoleResult = await userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());

            return createUserResult.Succeeded && assignRoleResult.Succeeded;
        }
    }
}
