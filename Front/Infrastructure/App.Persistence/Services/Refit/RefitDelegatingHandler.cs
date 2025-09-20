#region

    using System.IdentityModel.Tokens.Jwt;
    using System.Net;
    using System.Net.Http.Headers;
    using App.Applications.Users.Commands.Logout;
    using App.Applications.Users.Queries.GetUserInfo;
    using App.Common.Exceptions;
    using App.Common.General;
    using App.Common.Utilities.LifeTime;
    using App.Persistence.Services.Tenants;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Localization;

#endregion

    namespace App.Persistence.Services.Refit;

    public class RefitDelegatingHandler : DelegatingHandler,
        ITransientDependency
    {
        private readonly IStringLocalizer<RefitDelegatingHandler> _localizer;
        private readonly IServiceScopeFactory                     _scopeFactory;
        private readonly TenantStateProvider                      _tenantStateProvider;


        public RefitDelegatingHandler(
            IStringLocalizer<RefitDelegatingHandler> localizer,
            IServiceScopeFactory                     scopeFactory,
            TenantStateProvider tenantStateProvider)
        {
            _localizer         = localizer;
            _scopeFactory = scopeFactory;
            _tenantStateProvider = tenantStateProvider;

            InnerHandler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request,            nameof(request));
            ArgumentNullException.ThrowIfNull(request.RequestUri, nameof(request.RequestUri));


            try
            {
                var token = await AddTokenHeaderAsync(request.Headers);
                request.RequestUri = new Uri(UpdateUrlAsync(request.RequestUri.AbsoluteUri, _tenantStateProvider.CurrentTenant!));

                var response = await base.SendAsync(request, cancellationToken);

                if (response.StatusCode != HttpStatusCode.Unauthorized || string.IsNullOrEmpty(token))
                    return response;

                using var scope    = _scopeFactory.CreateScope();
                var       mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new LogoutCommand(token), cancellationToken);

                return response;
            }
            catch (HttpRequestException)
            {
                throw ShiftyException.Create(_localizer["NetworkError"]);
            }
            catch (TaskCanceledException)
            {
                throw ShiftyException.Create(_localizer["TimeoutError"]);
            }
            catch (Exception e)
            {
                throw ShiftyException.Create(_localizer["UnHandlerError"]);
            }
        }

        private async Task<string> AddTokenHeaderAsync(HttpRequestHeaders headers)
        {
            using var scope    = _scopeFactory.CreateScope();
            var       mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var       user     = await mediator.Send(new GetUserInfoQuery());

            headers.Add(ApplicationConstants.Headers.DeviceType, "Browser");

            if (string.IsNullOrEmpty(user?.Token) || IsTokenExpired(user.Token))
                return user?.Token ?? "";


            headers.Authorization = new AuthenticationHeaderValue(ApplicationConstants.Headers.Token, user.Token);
            return  user.Token;

        }

        private string UpdateUrlAsync(string requestUrl , string companyId = null!)
        {
            if (!string.IsNullOrEmpty(companyId))
                return requestUrl.Replace(
                    ApplicationConstants.Server.ServerUrl ,
                    $"{companyId}.{ApplicationConstants.Server.ServerUrl}" ,
                    StringComparison.OrdinalIgnoreCase);

            return requestUrl;
        }

        private bool IsTokenExpired(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken   = jwtHandler.ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.Now;
        }
    }