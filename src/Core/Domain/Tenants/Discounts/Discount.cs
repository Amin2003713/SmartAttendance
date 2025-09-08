using SmartAttendance.Common.General.Enums.Discount;

namespace SmartAttendance.Domain.Tenants.Discounts;

public class Discount
{
    public string? Code { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int Duration { get; set; }

    public DiscountType DiscountType { get; set; }

    public decimal Value { get; set; }
    public int? PackageMonth { get; set; }
    public virtual List<Payments.Payments> Payments { get; set; } = [];
    public virtual List<TenantDiscount> TenantDiscount { get; set; } = [];


    public bool IsPrivate { get; set; } = true;


    public bool IsActive { get; set; } = true;
    public Guid? CreatedBy { get; set; } = null;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? ModifiedBy { get; set; } = null;
    public DateTime? ModifiedAt { get; set; } = null!;
    public Guid? DeletedBy { get; set; } = null!;
    public DateTime? DeletedAt { get; set; } = null!;
    public Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);
}