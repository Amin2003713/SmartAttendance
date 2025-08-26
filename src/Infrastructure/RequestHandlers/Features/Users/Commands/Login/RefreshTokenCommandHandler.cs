using Microsoft.AspNetCore.Identity;
using Shifty.Application.Features.Users.Commands.RefreshToken;
using Shifty.Application.Interfaces.Jwt;
using Shifty.Common.Exceptions;
using Shifty.Common.Utilities.IdentityHelpers;
using Shifty.Domain.Users;
using Shifty.Persistence.Jwt;

namespace Shifty.RequestHandlers.Features.Users.Commands.Login;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IJwtService                                  _jwtService;
    private readonly IStringLocalizer<RefreshTokenCommandHandler> _localizer;
    private readonly ILogger<RefreshTokenCommandHandler>          _logger;
    private readonly IRefreshTokenQueryRepository                 _refreshTokenQueryRepository;
    private readonly IRefreshTokenCommandRepository               _refreshTokenRepository;
    private readonly UserManager<User>                            _userManager;

    public RefreshTokenCommandHandler(
        UserManager<User> userManager,
        IJwtService jwtService,
        IRefreshTokenCommandRepository refreshTokenRepository,
        ILogger<RefreshTokenCommandHandler> logger,
        IStringLocalizer<RefreshTokenCommandHandler> localizer,
        IRefreshTokenQueryRepository refreshTokenQueryRepository)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));

        _refreshTokenRepository =
            refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));

        _logger = logger;
        _localizer = localizer;
        _refreshTokenQueryRepository = refreshTokenQueryRepository;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var userId = await _jwtService.ValidateJwtAccessTokenAsync(request.AccessToken);

            if (userId.userId == null)
                throw IpaException.Unauthorized(_localizer["Invalid access token."]
                    .Value); // "توکن دسترسی نامعتبر است."

            var refreshToken = new UserToken
            {
                UserId = userId.userId.Value,
                RefreshToken = request.RefreshToken,
                AccessToken = null!,
                UniqueId = userId.uniq.Value
            };

            await _refreshTokenQueryRepository.ValidateRefreshTokenAsync(refreshToken, cancellationToken);

            var user = await _userManager.FindByIdAsync(userId.userId.ToString()!);

            if (user == null)
                throw IpaException.NotFound(_localizer["User was not found."].Value); // "کاربر یافت نشد."

            var uniqueId = Guid.CreateVersion7(DateTimeOffset.Now);
            var jwt      = await _jwtService.GenerateAsync(user, uniqueId.ToString());

            var updateRefreshToken = new UserToken
            {
                UserId = user.Id,
                ExpiryTime = DateTime.UtcNow.AddDays(jwt.refreshToken_expiresIn),
                RefreshToken = jwt.refresh_token,
                AccessToken = jwt.access_token.ComputeSha256Hash(),
                UniqueId = uniqueId
            };

            await _refreshTokenRepository.AddOrUpdateRefreshTokenAsync(updateRefreshToken, cancellationToken);

            return new RefreshTokenResponse
            {
                AccessToken = jwt.access_token,
                RefreshToken = jwt.refresh_token
            };
        }
        catch (IpaException e)
        {
            _logger.LogError(e, "Error occurred during token refresh.");
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while refreshing token.");
            throw IpaException.Unauthorized(_localizer["Unauthorized request."].Value); // "درخواست غیرمجاز."
        }
    }
}