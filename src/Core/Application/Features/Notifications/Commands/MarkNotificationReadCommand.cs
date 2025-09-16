using SmartAttendance.Domain.NotificationAggregate;

namespace SmartAttendance.Application.Features.Notifications.Commands;

// Command: علامت‌گذاری اعلان به عنوان خوانده
public sealed record MarkNotificationReadCommand(Guid NotificationId) : IRequest;

public sealed class MarkNotificationReadCommandHandler(INotificationRepository repo, IUnitOfWork uow)
	: IRequestHandler<MarkNotificationReadCommand>
{
	public async Task Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
	{
		var notif = await repo.GetByIdAsync(new NotificationId(request.NotificationId), cancellationToken)
			?? throw new KeyNotFoundException("اعلان یافت نشد.");
		notif.MarkAsRead();
		await uow.SaveChangesAsync(cancellationToken);
	}
}

