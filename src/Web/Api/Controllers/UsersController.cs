using SmartAttendance.Application.Features.Users.Commands;
using SmartAttendance.Application.Features.Users.Queries;
using SmartAttendance.Application.Features.Users.Requests;
using SmartAttendance.Application.Features.Users.Responses;

namespace SmartAttendance.Api.Controllers;

/// <summary>
///     عملیات مرتبط با کاربران سامانه.
/// </summary>
public sealed class UsersController : SmartAttendanceBaseController
{
	/// <summary>
	///     ایجاد کاربر جدید.
	/// </summary>
	/// <param name="request">درخواست ایجاد کاربر</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>اطلاعات پروفایل کاربر ایجاد شده</returns>
	[HttpPost]
    [ProducesResponseType(typeof(UserProfileDto),    StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserProfileDto>> CreateAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateUserCommand(request), cancellationToken);
        return Ok(result);
    }

	/// <summary>
	///     دریافت پروفایل کاربر بر اساس شناسه.
	/// </summary>
	/// <param name="userId">شناسه کاربر</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>اطلاعات پروفایل کاربر</returns>
	[HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(UserProfileDto),    StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserProfileDto>> GetByIdAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserProfileQuery(userId), cancellationToken);
        return Ok(result);
    }

	/// <summary>
	///     ویرایش پروفایل کاربر.
	/// </summary>
	/// <param name="userId">شناسه کاربر</param>
	/// <param name="request">درخواست ویرایش</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>پروفایل به‌روز شده</returns>
	[HttpPut("{userId:guid}")]
    [ProducesResponseType(typeof(UserProfileDto),    StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserProfileDto>> UpdateAsync(
        [FromRoute] Guid userId,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateUserCommand(userId, request), cancellationToken);
        return Ok(result);
    }

	/// <summary>
	///     حذف کاربر.
	/// </summary>
	/// <param name="userId">شناسه کاربر</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteUserCommand(userId), cancellationToken);
        return Ok();
    }

	/// <summary>
	///     شروع فرآیند فراموشی رمز عبور.
	/// </summary>
	/// <param name="request">درخواست فراموشی رمز</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[AllowAnonymous]
    [HttpPut("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new ForgotPasswordCommand(request), cancellationToken);
        return Ok();
    }

	/// <summary>
	///     بازنشانی رمز عبور با توکن معتبر.
	/// </summary>
	/// <param name="request">درخواست بازنشانی رمز</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[AllowAnonymous]
    [HttpPut("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new ResetPasswordCommand(request), cancellationToken);
        return Ok();
    }
}