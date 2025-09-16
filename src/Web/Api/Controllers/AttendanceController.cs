using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartAttendance.ApiFramework.Controller;
using SmartAttendance.Application.Features.Attendance.Commands;
using SmartAttendance.Application.Features.Attendance.Queries;
using SmartAttendance.Application.Features.Attendance.Requests;
using SmartAttendance.Application.Features.Attendance.Responses;

namespace SmartAttendance.Web.Api.Controllers;

/// <summary>
/// عملیات مربوط به حضور و غیاب.
/// </summary>
public sealed class AttendanceController : SmartAttendanceBaseController
{
	/// <summary>
	/// ثبت حضور (QR/GPS/دستی/آفلاین) بر اساس ورودی درخواست.
	/// </summary>
	/// <param name="request">درخواست ثبت حضور</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>اطلاعات حضور ثبت‌شده</returns>
	[HttpPost]
	[ProducesResponseType(typeof(AttendanceDto), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<AttendanceDto>> RecordAsync([FromBody] RecordAttendanceRequest request, CancellationToken cancellationToken)
	{
		var result = await Mediator.Send(new RecordAttendanceCommand(request), cancellationToken);
		return Ok(result);
	}

	/// <summary>
	/// مشاهده وضعیت حضور دانشجو در طرح.
	/// </summary>
	/// <param name="studentId">شناسه دانشجو</param>
	/// <param name="planId">شناسه طرح</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>وضعیت حضور</returns>
	[HttpGet("{studentId:guid}/{planId:guid}")]
	[ProducesResponseType(typeof(AttendanceDto), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<AttendanceDto>> GetStatusAsync([FromRoute] Guid studentId, [FromRoute] Guid planId, CancellationToken cancellationToken)
	{
		var result = await Mediator.Send(new GetAttendanceStatusQuery(studentId, planId), cancellationToken);
		return Ok(result);
	}

	/// <summary>
	/// تایید معذوریت حضور.
	/// </summary>
	/// <param name="attendanceId">شناسه حضور</param>
	/// <param name="request">درخواست تایید معذوریت</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpPut("{attendanceId:guid}/approve-excuse")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> ApproveExcuseAsync([FromRoute] Guid attendanceId, [FromBody] ApproveExcuseRequest request, CancellationToken cancellationToken)
	{
		await Mediator.Send(new ApproveExcuseCommand(attendanceId, request), cancellationToken);
		return Ok();
	}
}

