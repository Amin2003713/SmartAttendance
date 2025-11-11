namespace SmartAttendance.Application.Features.Notifications.Commands;

public class NotificationResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public bool IsRead { get; set; }
    public DateTime CreatedOn { get; set; }
}