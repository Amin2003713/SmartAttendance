using SmartAttendance.Application.Features.Plans.Commands;
using SmartAttendance.Application.Features.Plans.Queries;
using SmartAttendance.Application.Features.Plans.Requests;
using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Api.Controllers;

/// <summary>
///     عملیات مربوط به طرح‌ها.
/// </summary>
public sealed class PlansController : SmartAttendanceBaseController
{
	/// <summary>
	///     ایجاد طرح جدید.
	/// </summary>
	/// <param name="request">درخواست ایجاد طرح</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>اطلاعات طرح ایجاد شده</returns>
	[HttpPost]
    [ProducesResponseType(typeof(PlanDto),           StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PlanDto>> CreateAsync([FromBody] CreatePlanRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreatePlanCommand(request), cancellationToken);
        return Ok(result);
    }

	/// <summary>
	///     دریافت اطلاعات طرح بر اساس شناسه.
	/// </summary>
	/// <param name="planId">شناسه طرح</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>اطلاعات طرح</returns>
	[HttpGet("{planId:guid}")]
    [ProducesResponseType(typeof(PlanDto),           StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PlanDto>> GetByIdAsync([FromRoute] Guid planId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPlanByIdQuery(planId), cancellationToken);
        return Ok(result);
    }

	/// <summary>
	///     دریافت فهرست طرح‌ها.
	/// </summary>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>فهرست طرح‌ها</returns>
	[HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlanDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),    StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails),    StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails),    StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PlanDto>>> ListAsync(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListPlansQuery(), cancellationToken);
        return Ok(result);
    }

	/// <summary>
	///     ثبت‌نام دانشجو در طرح.
	/// </summary>
	/// <param name="planId">شناسه طرح</param>
	/// <param name="studentId">شناسه دانشجو</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpPost("{planId:guid}/enroll/{studentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EnrollAsync([FromRoute] Guid planId, [FromRoute] Guid studentId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new EnrollStudentCommand(planId, studentId), cancellationToken);
        return Ok();
    }

	/// <summary>
	///     به‌روزرسانی اطلاعات طرح.
	/// </summary>
	/// <param name="planId">شناسه طرح</param>
	/// <param name="request">درخواست به‌روزرسانی</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpPut("{planId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid planId, [FromBody] UpdatePlanRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdatePlanCommand(planId, request), cancellationToken);
        return Ok();
    }

	/// <summary>
	///     حذف طرح.
	/// </summary>
	/// <param name="planId">شناسه طرح</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpDelete("{planId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid planId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeletePlanCommand(planId), cancellationToken);
        return Ok();
    }

	/// <summary>
	///     انصراف دانشجو از طرح.
	/// </summary>
	/// <param name="planId">شناسه طرح</param>
	/// <param name="studentId">شناسه دانشجو</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpPut("{planId:guid}/cancel-enrollment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CancelEnrollmentAsync([FromRoute] Guid planId, [FromQuery] Guid studentId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new CancelEnrollmentCommand(planId, studentId), cancellationToken);
        return Ok();
    }
}