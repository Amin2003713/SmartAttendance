using SmartAttendance.Application.Features.Notifications.Responses;
using SmartAttendance.Domain.NotificationAggregate;

namespace SmartAttendance.Application.Features.Notifications.Commands;

public sealed class CreateNotificationCommandHandler(
    INotificationRepository repo,
    IUnitOfWork uow
)
    : IRequestHandler<CreateNotificationCommand, NotificationDto>
{
    public async Task<NotificationDto> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var r = request.Request;
        var channel = r.Channel?.Trim().ToLowerInvariant() switch
                      {
                          "email" => NotificationChannel.Email,
                          "sms"   => NotificationChannel.Sms,
                          "inapp" => NotificationChannel.InApp,
                          _       => throw new ArgumentException("کانال اعلان نامعتبر است.")
                      };

        var notif = new Notification(NotificationId.New(), new UserId(r.RecipientId), r.Title, r.Message, channel);
        await repo.AddAsync(notif, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return notif.Adapt<NotificationDto>();
    }
}