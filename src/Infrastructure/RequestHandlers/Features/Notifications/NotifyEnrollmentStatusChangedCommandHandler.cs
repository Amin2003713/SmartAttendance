using SmartAttendance.Application.Interfaces.Notifications;
using SmartAttendance.Common.General.Enums.Plans.Enrollment;
using SmartAttendance.Domain.Features.Notifications;

namespace SmartAttendance.Application.Features.Notifications.Commands;

public class NotifyEnrollmentStatusChangedCommandHandler(
    INotificationCommandRepository notificationCommandRepository
) : IRequestHandler<NotifyEnrollmentStatusChangedCommand>
{
    public async Task Handle(NotifyEnrollmentStatusChangedCommand request, CancellationToken cancellationToken)
    {
        var statusText = request.Status switch
                         {
                             EnrollmentStatus.Active     => "ثبت‌نام شما تایید شد و فعال است",
                             EnrollmentStatus.Cancelled  => "ثبت‌نام شما لغو شد",
                             EnrollmentStatus.Waitlisted => "در لیست انتظار برای این برنامه قرار گرفتید",
                             _                           => "وضعیت ثبت‌نام شما تغییر کرد"
                         };

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = request.StudentId,
            Title = "تغییر وضعیت ثبت‌نام",
            Message = $"{statusText} (برنامه: {request.PlanId})",
            IsRead = false,
            CreatedOn = DateTime.UtcNow
        };

        await notificationCommandRepository.AddAsync(notification, cancellationToken);
    }
}