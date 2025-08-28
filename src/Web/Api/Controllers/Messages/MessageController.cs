using Shifty.Application.Features.Messages.Commands.CreateMessage;
using Shifty.Application.Features.Messages.Commands.DeleteMassage;
using Shifty.Application.Features.Messages.Commands.LikeMessage;
using Shifty.Application.Features.Messages.Commands.UpdateMessage;
using Shifty.Application.Features.Messages.Commands.VisitMessage;
using Shifty.Application.Features.Messages.Queries.GetMessage;
using Shifty.Application.Features.Messages.Queries.GetMessageById;
using Shifty.Application.Features.Messages.Request.Commands.CreateMessage;
using Shifty.Application.Features.Messages.Request.Commands.UpdateMessage;
using Shifty.Application.Features.Messages.Request.Queries.GetMessage;
using Shifty.Application.Features.Messages.Request.Queries.GetMessageById;

namespace Shifty.Api.Controllers.Messages;

public class MessageController : IpaBaseController
{
    /// <summary>
    ///     Creates a new message for a project.
    /// </summary>
    /// <param name="request">The details required to create a project message.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A string confirmation of message creation.</returns>
    /// <remarks>
    ///     This endpoint accepts form-data and maps the request to a CreateMessageCommand.
    /// </remarks>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a message",
        Description = "Creates a new message ")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task CreateMessage([FromForm] CreateMessageRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateCommand>().AddFile(request.ImageFile!),
            cancellationToken);
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Put a message",
        Description = "Updates a message.")]
    [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task UpdateMessage([FromForm] UpdateMessageRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateMessageCommand>().AddFile(request.ImageFile!),
            cancellationToken);
    }


    /// <summary>
    ///     Retrieves messages associated with a specific project.
    /// </summary>
    /// <param name="projectId">The unique identifier of the project.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A list of messages corresponding to the project.</returns>
    /// <remarks>
    ///     This GET endpoint returns all messages that belong to the specified project id.
    /// </remarks>
    [HttpGet]
    [SwaggerOperation(Summary = "Get project messages",
        Description = "Retrieves a list of messages for a given project identified by its unique id.")]
    [ProducesResponseType(typeof(List<GetMessageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<List<GetMessageResponse>> GetMessage(Guid? projectId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetMessageQuery(), cancellationToken);
    }

    /// <summary>
    ///     Updates the like count for a specified message.
    /// </summary>
    /// <param name="id">The unique identifier of the message.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>No content if the operation is successful.</returns>
    /// <remarks>
    ///     This PUT endpoint increases the like count on a message.
    /// </remarks>
    [HttpPut("Like")]
    [SwaggerOperation(Summary = "Like a message",
        Description = "Increments the like counter for the message identified by its id.")]
    [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task UpdateLike(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new LikeMessageCommand(id), cancellationToken);
    }

    /// <summary>
    ///     Updates the visit count for a specified message.
    /// </summary>
    /// <param name="id">The unique identifier of the message.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>No content if the operation is successful.</returns>
    /// <remarks>
    ///     This PUT endpoint registers a visit for the provided message.
    /// </remarks>
    [HttpPut("Visit")]
    [SwaggerOperation(Summary = "Visit a message",
        Description = "Updates the visit count of a message to reflect that it has been viewed.")]
    public async Task UpdateVisit(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new VisitMessageCommand(id), cancellationToken);
    }

    /// <summary>
    ///     Deletes a specified message.
    /// </summary>
    /// <param name="id">The unique identifier of the message to be deleted.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <remarks>
    ///     This DELETE endpoint removes the message identified by its id from the system.
    /// </remarks>
    [HttpDelete]
    [SwaggerOperation(Summary = "Delete a message", Description = "Deletes the message specified by its unique id.")]
    [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task DeleteMessage(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteMessageCommand(id), cancellationToken);
    }


    [HttpGet("Get-By-Id")]
    [SwaggerOperation(Summary = "Get message By Id", Description = "Retrieves a  messages by its unique id.")]
    [ProducesResponseType(typeof(GetMessageByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<GetMessageByIdResponse> GetMessageById(
        Guid id,
        Guid? projectId,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetMessageByIdQuery(id), cancellationToken);
    }
}