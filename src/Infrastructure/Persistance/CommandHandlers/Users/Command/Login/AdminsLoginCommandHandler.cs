using MediatR;
using Shifty.Application.Users.Command.Login;
using Shifty.Common;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Users;
using Shifty.Domain.Users;
using Shifty.Persistence.Jwt;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Users.Command.Login;

public class AdminsLoginCommandHandler(ITenantAdminRepository userManager, IJwtServiceForTenant jwtService)
    : IRequestHandler<AdminsLoginCommand, LoginResponse>                                    
{


    public async Task<LoginResponse> Handle(AdminsLoginCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new InvalidNullInputException(nameof(request));

        var user = await userManager.GetByUsernameAsync(request.Username , cancellationToken);
        if (user == null)
            throw new ShiftyException(ApiResultStatusCode.NotFound, "username or password is incorrect");

        var isPasswordValid =  userManager.ValidatePasswordAsync(user, request.Password , cancellationToken);
        if (!isPasswordValid)
            throw new ShiftyException(ApiResultStatusCode.UnAuthorized, "username or password is incorrect");

        var jwt = await jwtService.GenerateAsync(user);

        var refreshToken = new RefreshToken
        {
            UserId = user.Id, ExpiryTime = DateTime.Now.AddDays(jwt.expires_in), Token = jwt.refresh_token
        };

        return new LoginResponse
        {
            RefreshToken = refreshToken.Token, Token = jwt.access_token, UserInfo = UserInfo.CreateInstance(user, [])
        };
    }
}