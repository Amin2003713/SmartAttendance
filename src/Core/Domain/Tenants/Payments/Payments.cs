namespace Shifty.Domain.Tenants.Payments;

public class Payments
{
    public Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);
    public bool IsActive { get; set; } = false;

    public DateTime PaymentDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int UsersCount { get; set; }
    public int ActiveUsers { get; set; }
    public int? ProjectsCount { get; set; } = 5;

    public decimal Cost { get; set; }
    public decimal BasePrice { get; set; } = 0;
    public decimal DiscountAmount { get; set; } = 0;
    public decimal TaxAmount { get; set; } = 0;
    public decimal GrantedStorageMb { get; set; } = 5120;

    public int Status { get; set; }
    public PaymentType PaymentType { get; set; }
    public int? Duration { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Guid UserId { get; set; }
    public string? Authority { get; set; }
    public string? RefId { get; set; }
    public Guid? LastPaymentId { get; set; }
    public Guid? PriceId { get; set; }
    public Guid? DiscountId { get; set; }
    public string TenantId { get; set; }

    public virtual Discount Discount { get; set; }
    public virtual Price Price { get; set; }
    public virtual Payments LastPayment { get; set; }
    public virtual ShiftyTenantInfo Tenant { get; set; }
    public virtual ICollection<ActiveService> ActiveServices { get; set; } = [];


    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public Guid? ModifiedBy { get; set; }
    public DateTime ModifiedAt { get; private set; } = DateTime.UtcNow;
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }

    public int LeftDays()
    {
        return Math.Max(0, (EndDate - DateTime.UtcNow.Date).Days);
    }
}