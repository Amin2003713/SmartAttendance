using App.Common.Utilities.LifeTime;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace App.Persistence.Services.Tenants;

public abstract class TenantStateProvider : IDisposable,
    IScopedDependency
{
    private readonly NavigationManager _navigationManager;

    public event Action<string>? TenantChanged;

    public string? CurrentTenant { get; private set; }

    protected TenantStateProvider(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        // Initialize once at startup
        UpdateTenantFromUri(_navigationManager.Uri);

        // Listen to navigation changes
        _navigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        UpdateTenantFromUri(e.Location);
    }

    private void UpdateTenantFromUri(string uri)
    {
        try
        {
            var host     = new Uri(uri).Host; // e.g., "tenant1.domain.com"
            var segments = host.Split('.');

            // assume tenant is always the first segment
            var tenantKey = segments.Length > 2 ? segments[0] : null;

            if (string.Equals(CurrentTenant, tenantKey, StringComparison.OrdinalIgnoreCase))
                return;

            CurrentTenant = tenantKey;
            TenantChanged?.Invoke(CurrentTenant!);
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