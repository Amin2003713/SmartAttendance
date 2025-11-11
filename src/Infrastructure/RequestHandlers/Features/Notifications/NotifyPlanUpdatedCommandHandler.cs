using SmartAttendance.Application.Interfaces.Notifications;
using SmartAttendance.Domain.Features.Notifications;

namespace SmartAttendance.Application.Features.Notifications.Commands;

public class NotifyPlanUpdatedCommandHandler(
    INotificationCommandRepository notificationCommandRepository
) : IRequestHandler<NotifyPlanUpdatedCommand>
{
    public async Task Handle(NotifyPlanUpdatedCommand request, CancellationToken cancellationToken)
    {
        var notifications = request.ToUser.Select(userId =>
            {
                var changes = new List<string>();
                if (request.IsTimeChanged)
                    changes.Add($"زمان: {request.NewStart:HH:mm} - {request.NewEnd:HH:mm}");

                if (request.IsLocationChanged)
                    changes.Add($"مکان: {request.NewLocation} - {request.NewAddress}");

                return new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Title = "به‌روزرسانی برنامه",
                    Message = $"برنامه {request.PlanId} به‌روزرسانی شد. تغییرات: {string.Join(", ", changes)}",
                    IsRead = false,
                    CreatedOn = DateTime.UtcNow
                };
            })
            .ToList();

        await notificationCommandRepository.AddRangeAsync(notifications, cancellationToken);
    }
}