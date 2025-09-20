using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using App.Applications.Users.Commands.Logout;
using App.Applications.Users.Queries.GetUserInfo;
using App.Common.Utilities.LifeTime;
using App.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace App.Persistence.Services.AuthState;

public class ClientStateProvider(
    IMediator mediator,
    NavigationManager navigationManager
) : AuthenticationStateProvider, IScopedDependency
{
    public UserInfo? User { get;private set; }
    private Type? _currentPageType;

    public void SetCurrentPageType(Type pageType) => _currentPageType = pageType;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Get user info from API if not loaded
            User ??= await mediator.Send(new GetUserInfoQuery());

            // Allow anonymous pages
            if (_currentPageType != null && _currentPageType.GetCustomAttribute<AllowAnonymousAttribute>() != null)
            {
                return User?.Token != null && !IsTokenExpired(User.Token)
                    ? new AuthenticationState(CreatePrincipal(User.Token))
                    : Anonymous();
            }

            // Protected pages
            if (User?.Token == null || IsTokenExpired(User.Token))
            {
                navigationManager.NavigateTo("/login");
                return Anonymous();
            }

            return new AuthenticationState(CreatePrincipal(User.Token));
        }
        catch
        {
            if (_currentPageType != null && _currentPageType.GetCustomAttribute<AllowAnonymousAttribute>() != null)
                return Anonymous();

            navigationManager.NavigateTo("/login");
            return Anonymous();
        }
    }

    private static AuthenticationState Anonymous() =>
        new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    private static bool IsTokenExpired(string token) =>
        new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo <= DateTime.UtcNow;

    private static ClaimsPrincipal CreatePrincipal(string token)
    {
        var jwt      = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        return new ClaimsPrincipal(identity);
    }

    public async Task Logout()
    {
        if (User?.Token != null)
        {
            try { await mediator.Send(new LogoutCommand(User.Token)); } catch { }
        }

        User = null;
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous()));
        navigationManager.NavigateTo("/login");
    }

    public Task SetUserAsync(UserInfo? user)
    {
        User = user;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }
}