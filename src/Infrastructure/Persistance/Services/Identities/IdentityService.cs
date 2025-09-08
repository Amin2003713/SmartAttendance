using System.Globalization;
using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Persistence.Services.Identities;

public class IdentityService(
    IHttpContextAccessor accessor,
    IMultiTenantContextAccessor<SmartAttendanceTenantInfo> contextAccessor
)
{
    private ClaimsPrincipal? Identity => accessor.HttpContext?.User;
    public SmartAttendanceTenantInfo? TenantInfo => contextAccessor.MultiTenantContext.TenantInfo;

    public bool IsAuthenticated => Identity != null;

    public string FindFirstValue(string claimType)
    {
        return Identity?.FindFirst(claimType)?.Value!;
    }

    public string GetValue(Predicate<Claim> predicate)
    {
        return Identity?.FindFirst(predicate)?.Value ??
               string.Empty;
    }

    public Guid? GetUserId()
    {
        var userIdClaim = Identity?.FindFirst("id");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var id) || id == Guid.Empty) return null;

        return id;
    }

    public Guid? GetUniqueId()
    {
        var userIdClaim = Identity?.FindFirst("uniqueTokenIdentifier");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var id) || id == Guid.Empty) return null;

        return id;
    }

    public T GetUserId<T>()
    {
        var userId = GetUserId();
        return (userId.HasValue ? (T)Convert.ChangeType(userId, typeof(T), CultureInfo.InvariantCulture) : default)!;
    }

    public string GetUserName()
    {
        return Identity?.FindFirst("username")?.Value;
    }

    public string GetFirstName()
    {
        return Identity?.FindFirst("firstName")?.Value;
    }

    public string GetLastName()
    {
        return Identity?.FindFirst("lastName")?.Value;
    }

    public string GetProfile()
    {
        return Identity?.FindFirst("profile")?.Value;
    }

    public DateTime? GetLastLoginDate()
    {
        var lastLoginDate = Identity?.FindFirst("lastLoginDate")?.Value;
        return DateTime.TryParse(lastLoginDate, out var date) ? date : null;
    }

    public List<Roles> GetRoles()
    {
        var roleClaims = Identity?
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        if (roleClaims == null || !roleClaims.Any())
            return new List<Roles>();

        var roles = new List<Roles>();

        foreach (var roleName in roleClaims.SelectMany(claim =>
                     claim.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)))
        {
            if (Enum.TryParse<Roles>(roleName, true, out var role))
                roles.Add(role);
        }

        return roles;
    }

    public string GetCustomClaim(string claimType)
    {
        return Identity?.FindFirst(claimType)?.Value!;
    }

    public string GetFullName()
    {
        return GetFirstName() + " " + GetLastName();
    }
}