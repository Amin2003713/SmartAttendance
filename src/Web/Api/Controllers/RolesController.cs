using SmartAttendance.Application.Features.Roles.Commands;
using SmartAttendance.Application.Features.Roles.Queries;
using SmartAttendance.Application.Features.Roles.Requests;
using SmartAttendance.Application.Features.Roles.Responses;

namespace SmartAttendance.Api.Controllers;

/// <summary>
///     عملیات مرتبط با نقش‌ها.
/// </summary>
public sealed class RolesController : SmartAttendanceBaseController
{
	/// <summary>
	///     انتساب نقش به کاربر.
	/// </summary>
	/// <param name="request">درخواست انتساب نقش</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpPost("assign")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignAsync([FromBody] AssignRoleRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new AssignRoleCommand(request), cancellationToken);
        return Ok();
    }

	/// <summary>
	///     ایجاد نقش جدید.
	/// </summary>
	/// <param name="request">درخواست ایجاد نقش</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new CreateRoleCommand(request), cancellationToken);
        return Ok();
    }

	/// <summary>
	///     دریافت نقش بر اساس شناسه.
	/// </summary>
	/// <param name="id">شناسه نقش</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>اطلاعات نقش</returns>
	[HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RoleDto),           StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RoleDto>> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetRoleByIdQuery(id), cancellationToken);
        return Ok(result);
    }

	/// <summary>
	///     دریافت فهرست نقش‌ها.
	/// </summary>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>فهرست نقش‌ها</returns>
	[HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),    StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails),    StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails),    StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<RoleDto>>> ListAsync(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListRolesQuery(), cancellationToken);
        return Ok(result);
    }
}