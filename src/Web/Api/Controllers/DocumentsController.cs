using SmartAttendance.Application.Features.Documents.Commands;
using SmartAttendance.Application.Features.Documents.Queries;
using SmartAttendance.Application.Features.Documents.Requests;
using SmartAttendance.Application.Features.Documents.Responses;

namespace SmartAttendance.Api.Controllers;

/// <summary>
///     عملیات مربوط به اسناد.
/// </summary>
public sealed class DocumentsController : SmartAttendanceBaseController
{
	/// <summary>
	///     ثبت متادیتای سند.
	/// </summary>
	/// <param name="request">درخواست آپلود سند</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>اطلاعات سند ثبت‌شده</returns>
	[HttpPost]
    [ProducesResponseType(typeof(DocumentDto),       StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DocumentDto>> UploadAsync([FromBody] UploadDocumentRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UploadDocumentCommand(request), cancellationToken);
        return Ok(result);
    }

	/// <summary>
	///     دریافت سند بر اساس شناسه.
	/// </summary>
	/// <param name="id">شناسه سند</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>اطلاعات سند</returns>
	[HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DocumentDto),       StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DocumentDto>> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetDocumentByIdQuery(id), cancellationToken);
        return Ok(result);
    }

	/// <summary>
	///     تایید سند بارگذاری‌شده.
	/// </summary>
	/// <param name="documentId">شناسه سند</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpPost("{documentId:guid}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ApproveAsync([FromRoute] Guid documentId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new ApproveDocumentCommand(documentId), cancellationToken);
        return Ok();
    }

	/// <summary>
	///     رد سند بارگذاری‌شده.
	/// </summary>
	/// <param name="documentId">شناسه سند</param>
	/// <param name="cancellationToken">توکن لغو عملیات</param>
	/// <returns>نتیجه عملیات</returns>
	[HttpPost("{documentId:guid}/reject")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RejectAsync([FromRoute] Guid documentId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new RejectDocumentCommand(documentId), cancellationToken);
        return Ok();
    }
}