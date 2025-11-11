using MediatR;
using SmartAttendance.Application.Interfaces.Notifications;
using SmartAttendance.Domain.Features.Notifications;

namespace SmartAttendance.Application.Features.Notifications.Commands;

public class NotifyTimeHasChangedCommandHandler(
    INotificationCommandRepository notificationCommandRepository
) : IRequestHandler<NotifyTimeHasChengedCommand>
{
    public async Task Handle(NotifyTimeHasChengedCommand request, CancellationToken cancellationToken)
    {
        var notifications = request.ToUser.Select(userId => new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = "تغییر زمان برنامه",
                Message
                    = $"زمان برنامه {request.PlanId} تغییر کرده است. زمان شروع جدید: {request.NewDateStart:HH:mm}، زمان پایان جدید: {request.NewDateEnd:HH:mm}",
                IsRead = false,
                CreatedOn = DateTime.UtcNow
            })
            .ToList();

        await notificationCommandRepository.AddRangeAsync(notifications, cancellationToken);
    }
}