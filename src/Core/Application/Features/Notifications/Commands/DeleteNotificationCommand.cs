namespace SmartAttendance.Application.Features.Notifications.Commands;

public sealed record DeleteNotificationCommand(
    Guid Id
) : IRequest;