using App.Applications.Universities.Queries.GetUniversityInfo;
using App.Applications.Universities.Responses.GetCompanyInfo;
using App.Common.Utilities.LifeTime;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace App.Persistence.Services.Tenants;

public class TenantStateProvider : IDisposable,
    ISingletonDependency
{
    private readonly NavigationManager _navigationManager;
    private readonly IMediator         _mediator;

    public event Action<string>? TenantChanged;

    public string? CurrentTenant { get; private set; }
    public GetUniversityInfoResponse? University { get; private set; }

    public TenantStateProvider(NavigationManager navigationManager, IMediator mediator)
    {
        _navigationManager = navigationManager;
        _mediator = mediator;

        // Initialize once at startup
        _ = UpdateTenantFromUri(_navigationManager.Uri);

        // Listen to navigation changes
        _navigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _ = UpdateTenantFromUri(e.Location);
    }

    public async Task UpdateTenantFromUri(string uri)
    {
        try
        {
            var host     = new Uri(uri).Host; // e.g., "tenant1.domain.com"
            var segments = host.Split('.');

            // assume tenant is always the first segment
            var tenantKey = segments.Length > 1 ? segments[0] : null;

            if (string.Equals(CurrentTenant, tenantKey, StringComparison.OrdinalIgnoreCase))
                return;

            CurrentTenant = tenantKey;
            TenantChanged?.Invoke(CurrentTenant!);
            try
            {
                University = await _mediator.Send(new GetUniversityInfoQuery(tenantKey!));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        catch
        {
            CurrentTenant = null;
        }
    }

    public void Dispose()
    {
        _navigationManager.LocationChanged -= OnLocationChanged;
    }
}