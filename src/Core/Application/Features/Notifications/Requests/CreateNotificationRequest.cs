namespace SmartAttendance.Application.Features.Notifications.Requests;

// درخواست ایجاد اعلان
public sealed class CreateNotificationRequest
{
    public Guid RecipientId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string Channel { get; init; } = string.Empty; // Email/Sms/InApp
}