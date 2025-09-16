namespace SmartAttendance.Application.Features.Notifications.Commands;

// Command: علامت‌گذاری اعلان به عنوان خوانده
public sealed record MarkNotificationReadCommand(
    Guid NotificationId
) : IRequest;