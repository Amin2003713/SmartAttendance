using SmartAttendance.Application.Features.Notifications.Responses;

namespace SmartAttendance.Application.Features.Notifications.Queries;

public sealed record GetNotificationsByUserQuery(Guid UserId) : IRequest<IReadOnlyList<NotificationDto>>;

public sealed class GetNotificationsByUserQueryHandler(INotificationRepository repo) : IRequestHandler<GetNotificationsByUserQuery, IReadOnlyList<NotificationDto>>
{
	public async Task<IReadOnlyList<NotificationDto>> Handle(GetNotificationsByUserQuery request, CancellationToken cancellationToken)
	{
		var list = await repo.GetByUserAsync(new UserId(request.UserId), cancellationToken);
		return list.Adapt<IReadOnlyList<NotificationDto>>();
	}
}

