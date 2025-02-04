using System.Net;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shifty.Application.Users.Command.Login;
using Shifty.Common.Exceptions;
using Shifty.Domain.Features.Users;
using Shifty.Domain.Interfaces.Jwt;
using Shifty.Persistence.Jwt;
using Shifty.Resources.Messages;

namespace Shifty.RequestHandlers.Users.Commands.Login
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

                if (!user.PhoneNumberConfirmed)
                    throw ShiftyException.Forbidden(messages.User_Error_NotActive());

                var isPasswordValid = await userManager.CheckPasswordAsync(user , request.Password);
                if (!isPasswordValid)
                    throw ShiftyException.Unauthorized(additionalData: messages.InCorrect_User_Password());

                var jwt = await jwtService.GenerateAsync(user);

                var refreshToken = new RefreshToken
                {
                    UserId = user.Id , ExpiryTime = DateTime.Now.AddDays(jwt.expires_in) , Token = jwt.refresh_token ,
                };

                await refreshTokenRepository.AddOrUpdateRefreshTokenAsync(refreshToken , cancellationToken);

                return new LoginResponse()
                {
                    RefreshToken = refreshToken.Token ,
                    Token        = jwt.access_token ,
                };
            }
            catch (ShiftyException e)
            {
                logger.LogError(e.Source , e);
                throw;
            }
        }
    }
}