using SmartAttendance.Application.Features.Notifications.Requests;
using SmartAttendance.Application.Features.Notifications.Responses;

namespace SmartAttendance.Application.Features.Notifications.Commands;

// Command: ایجاد اعلان
public sealed record CreateNotificationCommand(
    CreateNotificationRequest Request
) : IRequest<NotificationDto>;

// Handler: ایجاد اعلان