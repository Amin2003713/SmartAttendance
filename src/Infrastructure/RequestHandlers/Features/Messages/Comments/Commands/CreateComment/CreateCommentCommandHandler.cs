using Mapster;
using Shifty.Application.Features.Messages.Comments.Commands.CreateComment;
using Shifty.Application.Interfaces.Messages.Comments;
using Shifty.Common.Exceptions;
using Shifty.Domain.Messages.Comments;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Messages.Comments.Commands.CreateComment;

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
        catch (ShiftyException ex)
        {
            logger.LogWarning(ex, "Business error while creating comment: {Message}", ex.Message);
            throw ShiftyException.BadRequest(localizer[ex.Message].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating comment");
            throw ShiftyException.InternalServerError(localizer["Unable to create comment."]);
        }
    }
}