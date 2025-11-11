using Microsoft.AspNetCore.Mvc;
using SmartAttendance.Application.Features.Notifications.Commands;


namespace SmartAttendance.Api.Controllers.Notifications;

[ApiController]
public class NotificationController : SmartAttendanceBaseController
{
    /// <summary>
    /// Get all notifications for the given user.
    /// </summary>
    /// <param name="userId">User ID to fetch notifications for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of notifications.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<NotificationResponse>), 200)]
    public async Task<List<NotificationResponse>> GetNotifications(
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetNotificationsByUserQuery(),
            cancellationToken);
    }
}