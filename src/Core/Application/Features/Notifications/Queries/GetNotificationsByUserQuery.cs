using SmartAttendance.Application.Features.Notifications.Responses;

namespace SmartAttendance.Application.Features.Notifications.Queries;

public sealed record GetNotificationsByUserQuery(
    Guid UserId
) : IRequest<IReadOnlyList<NotificationDto>>;