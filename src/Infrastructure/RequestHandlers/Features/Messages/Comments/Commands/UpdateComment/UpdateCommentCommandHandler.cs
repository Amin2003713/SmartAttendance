using SmartAttendance.Application.Features.Messages.Comments.Commands.UpdateComment;
using SmartAttendance.Application.Interfaces.Messages.Comments;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Messages.Comments.Commands.UpdateComment;

public class UpdateCommentCommandHandler(
    ICommentCommandRepository commandRepository,
    ICommentQueryRepository queryRepository,
    ILogger<UpdateCommentCommandHandler> logger,
    IStringLocalizer<UpdateCommentCommandHandler> localizer
) : IRequestHandler<UpdateCommentCommand>
{
    public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var comment = await queryRepository.GetSingleAsync(cancellationToken, x => x.Id == request.Id);

            if (comment == null)
            {
                logger.LogWarning("Comment with ID {CommentId} not found for update.", request.Id);
                throw SmartAttendanceException.NotFound(localizer["Comment not found."]);
            }

            comment.Text = request.Text;
            await commandRepository.UpdateAsync(comment, cancellationToken);

            logger.LogInformation("Comment Updated successfully.");
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogWarning(ex, "Business error while updating comment: {Message}", ex.Message);
            throw SmartAttendanceException.BadRequest(localizer[ex.Message].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating comment");
            throw SmartAttendanceException.InternalServerError(localizer["Unable to updating comment."]);
        }
    }
}