namespace Shifty.Domain.Tenants;

public class TenantCalendar
{
    public bool IsActive { get; set; } = true;
    public Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);

    public DateTime Date { get; set; }
    public bool IsHoliday { get; set; } = false;
    public bool IsWeekend { get; set; } = false;
    public string? Details { get; set; } = null!;
}