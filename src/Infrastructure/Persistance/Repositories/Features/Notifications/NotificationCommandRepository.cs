using SmartAttendance.Application.Interfaces.Notifications;
using SmartAttendance.Domain.Features.Notifications;

namespace SmartAttendance.Persistence.Repositories.Features.Notifications;

public class NotificationCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<Notification>> logger
)
    : CommandRepository<Notification>(dbContext, logger),
        INotificationCommandRepository { }