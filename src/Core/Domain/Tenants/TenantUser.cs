namespace Shifty.Domain.Tenants;

public class TenantUser
{
    public Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string NationalCode { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public string ShiftyTenantInfoId { get; set; }
    public ShiftyTenantInfo ShiftyTenantInfo { get; set; }
    public bool IsActive { get; set; } = true;
    public string UserName { get; set; }
}