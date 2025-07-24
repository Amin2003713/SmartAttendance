namespace Shifty.Domain.Tenants;

public class TenantAdmin
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    public List<ShiftyTenantInfo> Tenants { get; set; } = [];
    public DateTime RegisteredAt { get; set; } = DateTime.Now;

    public string? FullName()
    {
        return string.Concat(FirstName + " ", LastName);
    }
}