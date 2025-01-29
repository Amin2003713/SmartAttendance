using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shifty.Application.Users.Command.Login;
using Shifty.Common;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Jwt;
using Shifty.Persistence.Jwt;
using Shifty.Resources.Messages;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Domain.Features.Users;

namespace Shifty.Persistence.CommandHandlers.Users.Command.Login
{
    public class LoginCommandHandler(UserManager<User> userManager , IJwtService jwtService , IRefreshTokenRepository refreshTokenRepository , ILogger<LoginCommandHandler> logger , UserMessages messages)
        : IRequestHandler<LoginCommand , LoginResponse>
    {
        public async Task<LoginResponse> Handle(LoginCommand request , CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                    throw new InvalidNullInputException(nameof(request));

                var user = await userManager.FindByNameAsync(request.Username);
                if (user == null)
                    throw ShiftyException.NotFound(additionalData: messages.User_NotFound());

                var isPasswordValid = await userManager.CheckPasswordAsync(user , request.Password);
                if (!isPasswordValid)
                    throw ShiftyException.Unauthorized(additionalData: messages.InCorrect_User_Password());

                var roles = await userManager.GetRolesAsync(user);

                var jwt = await jwtService.GenerateAsync(user);

                var refreshToken = new RefreshToken
                {
                    UserId = user.Id , ExpiryTime = DateTime.Now.AddDays(jwt.expires_in) , Token = jwt.refresh_token ,
                };

                await refreshTokenRepository.AddOrUpdateRefreshTokenAsync(refreshToken , cancellationToken);

                var loginResult = user.Adapt<LoginResponse>();

                return loginResult.AddToken(refreshToken.Token , jwt.access_token , roles.ToList() ?? []);
            }
            catch (ShiftyException e)
            {
                logger.LogError(e.Source , e);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Source , e);
                throw ShiftyException.InternalServerError();
            }
        }
    }
}