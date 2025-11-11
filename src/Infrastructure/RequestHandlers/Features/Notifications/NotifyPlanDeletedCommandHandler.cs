using SmartAttendance.Application.Interfaces.Notifications;
using SmartAttendance.Domain.Features.Notifications;

namespace SmartAttendance.Application.Features.Notifications.Commands;

public class NotifyPlanDeletedCommandHandler(
    INotificationCommandRepository notificationCommandRepository
) : IRequestHandler<NotifyPlanDeletedCommand>
{
    public async Task Handle(NotifyPlanDeletedCommand request, CancellationToken cancellationToken)
    {
        var notifications = request.ToUser.Select(userId => new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = "حذف برنامه",
                Message = $"برنامه {request.PlanId} حذف شد.",
                IsRead = false,
                CreatedOn = DateTime.UtcNow
            })
            .ToList();

        await notificationCommandRepository.AddRangeAsync(notifications, cancellationToken);
    }
}