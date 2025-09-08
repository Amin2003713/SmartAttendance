using SmartAttendance.Application.Features.Messages.Comments.Commands.CreateComment;
using SmartAttendance.Application.Features.Messages.Comments.Commands.UpdateComment;
using SmartAttendance.Application.Features.Messages.Comments.Queries.GetComments;
using SmartAttendance.Application.Features.Messages.Comments.Request.Commands.CreateComment;
using SmartAttendance.Application.Features.Messages.Comments.Request.Commands.UpdateComment;
using SmartAttendance.Application.Features.Messages.Comments.Request.Queries.GetComments;

namespace SmartAttendance.Api.Controllers.Messages.Comments;

public class CommentController : SmartAttendanceBaseController
{
    /// <summary>
    ///     Retrieves the list of comments for a specific message or item.
    /// </summary>
    /// <param name="id">The identifier of the message or entity to retrieve comments for.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Comments retrieved successfully.</response>
    /// <response code="400">Invalid identifier provided.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<GetCommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<List<GetCommentResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetCommentQuery(id), cancellationToken);
    }

    /// <summary>
    ///     Creates a new comment for a specific message or item.
    /// </summary>
    /// <param name="request">Requests containing comment details such as content and related message ID.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="201">Comment created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task CreateComment([FromBody] CreateCommentRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateCommentCommand>(), cancellationToken);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task UpdateComment([FromBody] UpdateCommentRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateCommentCommand>(), cancellationToken);
    }
}