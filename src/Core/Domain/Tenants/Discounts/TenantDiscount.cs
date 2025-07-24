namespace Shifty.Domain.Tenants.Discounts;

public class TenantDiscount
{
    public Guid Id { get; set; }

    public Guid DiscountId { get; set; }
    public Discount Discount { get; set; }

    public string TenantId { get; set; }
    public ShiftyTenantInfo Tenant { get; set; }
    public bool IsUsed { get; set; } = false;
}