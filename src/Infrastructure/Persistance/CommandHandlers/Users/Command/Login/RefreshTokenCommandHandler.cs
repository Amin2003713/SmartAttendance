using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shifty.Application.Users.Command.RefreshToken;
using Shifty.Common;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Jwt;
using Shifty.Domain.Users;
using Shifty.Persistence.Jwt;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Users.Command.Login
{
    public class RefreshTokenCommandHandler(UserManager<User> userManager , IJwtService jwtService , IRefreshTokenRepository refreshTokenRepository , ILogger<RefreshTokenCommandHandler> logger)
        : IRequestHandler<RefreshTokenCommand , RefreshTokenResponse>
    {
        private readonly IJwtService _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));

        private readonly IRefreshTokenRepository _refreshTokenRepository =
            refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));

        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request , CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                    throw new InvalidNullInputException(nameof(request));

                var userId = _jwtService.ValidateJwtAccessTokenAsync(request.AccessToken);
                if (userId == null)
                    throw ShiftyException.Unauthorized();

                var refreshToken = new RefreshToken
                {
                    UserId = userId.Value , Token = request.RefreshToken ,
                };
                await _refreshTokenRepository.ValidateRefreshTokenAsync(refreshToken , cancellationToken);

                var user = await _userManager.FindByIdAsync(userId.ToString());
                var jwt  = await _jwtService.GenerateAsync(user);
                var updateRefreshToken = new RefreshToken
                {
                    UserId = user!.Id , ExpiryTime = DateTime.Now.AddDays(jwt.refreshToken_expiresIn) , Token = jwt.refresh_token ,
                };
                await _refreshTokenRepository.AddOrUpdateRefreshTokenAsync(updateRefreshToken , cancellationToken);
                return new RefreshTokenResponse
                {
                    AccessToken = jwt.access_token , RefreshToken = jwt.refresh_token ,
                };
            }
            catch (Exception e)
            {
                logger.LogError(e.Source , e);
                throw ShiftyException.Unauthorized();
            }
        }
    }
}