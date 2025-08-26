using Microsoft.AspNetCore.Identity;
using Shifty.Application.Features.Users.Commands.Login;
using Shifty.Application.Interfaces.Jwt;
using Shifty.Common.Exceptions;
using Shifty.Common.Utilities.IdentityHelpers;
using Shifty.Domain.Users;
using Shifty.Persistence.Jwt;

namespace Shifty.RequestHandlers.Features.Users.Commands.Login;

public class LoginCommandHandler(
    UserManager<User> userManager,
    IJwtService jwtService,
    IRefreshTokenCommandRepository refreshTokenRepository,
    ILogger<LoginCommandHandler> logger,
    IStringLocalizer<LoginCommandHandler> localizer
) : IRequestHandler<LoginCommand, LoginResponse>
{
    // We’ll escalate lockout durations as follows:
    //   • 3rd failure → 5‐minute lockout
    //   • 4th failure → 30‐minute lockout
    //   • 5th (or more) failure → force user to contact support

    private readonly static TimeSpan FirstLockoutDuration  = TimeSpan.FromMinutes(10);
    private readonly static TimeSpan SecondLockoutDuration = TimeSpan.FromMinutes(60);

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            // 1. Look up the user by username.
            var user = await userManager.FindByNameAsync(request.UserName);

            if (user == null)
                throw IpaException.NotFound(localizer["User was not found."].Value);

            // 2. If Identity thinks they’re already locked out, tell them when it ends.
            if (await userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
                var localEnd   = lockoutEnd?.DateTime.ToLocalTime() ?? DateTime.UtcNow;

                if (localEnd.Date == DateTime.MaxValue.Date)
                    throw IpaException.Forbidden(
                        localizer["User account is locked dou To many failed login attempts. Please contact support."]
                            .Value);

                string msg = localizer["User account is locked until {0}.", localEnd.ToLocalTime()];
                throw IpaException.Forbidden(msg);
            }

            // 3. Require phone‐number confirmation as before.
            if (!user.PhoneNumberConfirmed)
                throw IpaException.Forbidden(localizer["User account is not activated."].Value);

            // 4. Validate password.
            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                // 4a. Increment the failed‐attempt counter.
                await userManager.AccessFailedAsync(user);

                // 4b. Determine how many consecutive failures have occurred.
                var failures = await userManager.GetAccessFailedCountAsync(user);

                if (failures == 3)
                {
                    // 3rd failure → lock out for 5 minutes.
                    var until = DateTimeOffset.UtcNow.Add(FirstLockoutDuration);
                    await userManager.SetLockoutEndDateAsync(user, until);

                    throw IpaException.Forbidden(
                        localizer["Account locked for {0} minutes after 3 failed attempts.",
                            FirstLockoutDuration.TotalMinutes].Value
                    );
                }

                if (failures == 4)
                {
                    // 4th failure → lock out for 30 minutes.
                    var until = DateTimeOffset.UtcNow.Add(SecondLockoutDuration);
                    await userManager.SetLockoutEndDateAsync(user, until);

                    throw IpaException.Forbidden(
                        localizer["Account locked for {0} minutes after 4 failed attempts.",
                            SecondLockoutDuration.TotalMinutes].Value
                    );
                }

                if (failures >= 5)
                {
                    // 4th failure → lock out for 30 minutes.
                    var until = DateTimeOffset.MaxValue.ToUniversalTime();
                    await userManager.SetLockoutEndDateAsync(user, until);

                    throw IpaException.Forbidden(localizer["Too many failed login attempts. Please contact support."]
                        .Value);
                }

                // 1st or 2nd failure → just say “invalid credentials.”
                throw IpaException.Unauthorized(localizer["Incorrect username or password."].Value);
            }

            // 5. Successful password: clear failure count immediately.
            await userManager.ResetAccessFailedCountAsync(user);

            // 6. Generate new GUID v7 and JWT pair.
            var uniqueId = Guid.CreateVersion7(DateTimeOffset.UtcNow);
            var jwt      = await jwtService.GenerateAsync(user, uniqueId.ToString());

            // 7. Persist/update the refresh token record.
            var refreshToken = new UserToken
            {
                UniqueId = uniqueId,
                AccessToken = jwt.access_token.ComputeSha256Hash(),
                RefreshToken = jwt.refresh_token,
                UserId = user.Id,
                ExpiryTime = DateTime.UtcNow.AddSeconds(jwt.expires_in)
            };

            await refreshTokenRepository.AddOrUpdateRefreshTokenAsync(refreshToken, cancellationToken);

            // 8. Return tokens.
            return new LoginResponse
            {
                Token = jwt.access_token,
                RefreshToken = refreshToken.RefreshToken
            };
        }
        catch (IpaException ex)
        {
            logger.LogError(ex, "Error occurred during login.");
            throw;
        }
    }
}