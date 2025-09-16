namespace SmartAttendance.Application.Features.Notifications.Commands;

public sealed class MarkNotificationReadCommandHandler(
    INotificationRepository repo,
    IUnitOfWork uow
)
    : IRequestHandler<MarkNotificationReadCommand>
{
    public async Task Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        var notif = await repo.GetByIdAsync(new NotificationId(request.NotificationId), cancellationToken) ?? throw new KeyNotFoundException("اعلان یافت نشد.");
        notif.MarkAsRead();
        await uow.SaveChangesAsync(cancellationToken);
    }
}