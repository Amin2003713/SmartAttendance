namespace SmartAttendance.Application.Features.Notifications.Responses;

// DTO نمایش اعلان
public sealed class NotificationDto
{
    public Guid Id { get; init; }
    public Guid RecipientId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string Channel { get; init; } = string.Empty;
    public bool IsSent { get; init; }
    public bool IsRead { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}