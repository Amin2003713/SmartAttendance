using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Interfaces.Notifications;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.Application.Features.Notifications.Commands;

public class GetNotificationsByUserQueryHandler(
    INotificationQueryRepository notificationQueryRepository   ,
    IdentityService identityService
) : IRequestHandler<GetNotificationsByUserQuery, List<NotificationResponse>>
{
    public async Task<List<NotificationResponse>> Handle(GetNotificationsByUserQuery request, CancellationToken cancellationToken)
    {
        var notifications = await notificationQueryRepository.TableNoTracking
            .Where(n => n.UserId == identityService.GetUserId<Guid>())
            .OrderByDescending(n => n.CreatedOn)
            .ToListAsync(cancellationToken);

        return notifications.Adapt<List<NotificationResponse>>();
    }
}