namespace SmartAttendance.Application.Features.Notifications.Commands;

public sealed class DeleteNotificationCommandHandler(
    INotificationRepository repo,
    IUnitOfWork uow
) : IRequestHandler<DeleteNotificationCommand>
{
    public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        await repo.DeleteAsync(new NotificationId(request.Id), cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
    }
}