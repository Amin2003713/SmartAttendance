using SmartAttendance.Application.Features.Notifications.Commands;
using SmartAttendance.Application.Features.Notifications.Queries;
using SmartAttendance.Application.Features.Notifications.Requests;
using SmartAttendance.Application.Features.Notifications.Responses;

namespace SmartAttendance.Api.Controllers;

/// <summary>
///     عملیات مربوط به اعلان‌ها.
/// </summary>
public sealed class NotificationsController : SmartAttendanceBaseController
{
	/// <summary>
	///     ایجاد اعلان جدید.
	/// </summary>
	/// <param name="request">درخواست ایجاد اعلان</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>اطلاعات اعلان ایجاد شده</returns>
	[HttpPost]
    [ProducesResponseType(typeof(NotificationDto),   StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<NotificationDto>> CreateAsync([FromBody] CreateNotificationRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateNotificationCommand(request), cancellationToken);
        return Ok(result);
    }

	/// <summary>
	///     دریافت اعلان‌های کاربر.
	/// </summary>
	/// <param name="userId">شناسه کاربر</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>فهرست اعلان‌ها</returns>
	[HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),            StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails),            StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails),            StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetByUserAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var list = await Mediator.Send(new GetNotificationsByUserQuery(userId), cancellationToken);
        return Ok(list);
    }

	/// <summary>
	///     حذف اعلان.
	/// </summary>
	/// <param name="id">شناسه اعلان</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteNotificationCommand(id), cancellationToken);
        return Ok();
    }

	/// <summary>
	///     علامت‌گذاری اعلان به عنوان خوانده شده.
	/// </summary>
	/// <param name="notificationId">شناسه اعلان</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpPost("{notificationId:guid}/read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MarkAsReadAsync([FromRoute] Guid notificationId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new MarkNotificationReadCommand(notificationId), cancellationToken);
        return Ok();
    }
}