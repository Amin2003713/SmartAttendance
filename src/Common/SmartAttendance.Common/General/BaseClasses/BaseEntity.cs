namespace SmartAttendance.Common.General.BaseClasses;

public class BaseEntity : IEntity
{
    public bool IsActive { get; set; } = true;
    public Guid? CreatedBy { get; set; } = null;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? ModifiedBy { get; set; } = null;
    public DateTime? ModifiedAt { get; set; } = null!;
    public Guid? DeletedBy { get; set; } = null!;
    public DateTime? DeletedAt { get; set; } = null!;
    public Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);
}