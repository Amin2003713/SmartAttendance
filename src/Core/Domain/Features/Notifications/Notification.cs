using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Features.Notifications;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = default!;
}