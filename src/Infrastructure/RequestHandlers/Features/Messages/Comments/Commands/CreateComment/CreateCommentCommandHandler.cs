using Mapster;
using SmartAttendance.Application.Features.Messages.Comments.Commands.CreateComment;
using SmartAttendance.Application.Interfaces.Messages.Comments;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Messages.Comments;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Messages.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler(
    ICommentCommandRepository commandRepository,
    IdentityService service,
    ILogger<CreateCommentCommandHandler> logger,
    IStringLocalizer<CreateCommentCommandHandler> localizer
)
    : IRequestHandler<CreateCommentCommand>
{
    public async Task Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = service.GetUserId<Guid>();

            var comment = request.Adapt<Comment>();
            comment.CreatedBy = userId;

            await commandRepository.AddAsync(comment, cancellationToken);

            logger.LogInformation("Comment created by UserId: {UserId}", userId);
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogWarning(ex, "Business error while creating comment: {Message}", ex.Message);
            throw SmartAttendanceException.BadRequest(localizer[ex.Message].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating comment");
            throw SmartAttendanceException.InternalServerError(localizer["Unable to create comment."]);
        }
    }
}