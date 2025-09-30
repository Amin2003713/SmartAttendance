using SmartAttendance.Application.Interfaces.Notifications;
using SmartAttendance.Domain.Features.Notifications;

namespace SmartAttendance.Persistence.Repositories.Features.Notifications;

public class NotificationQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<Notification>>          logger,
    IStringLocalizer<Notification> localizer
)
    : QueryRepository<Notification>(dbContext, logger),
        INotificationQueryRepository;