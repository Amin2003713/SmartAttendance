namespace Shifty.Domain.Tenants;

public class Price
{
    public Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);
    public decimal Amount { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid? CreatedBy { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }

    public virtual IEnumerable<Payments.Payments> Payments { get; set; } = new List<Payments.Payments>();
}