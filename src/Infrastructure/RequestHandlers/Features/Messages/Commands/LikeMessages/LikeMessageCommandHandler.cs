using Mapster;
using Shifty.Application.Features.Messages.Commands.LikeMessage;
using Shifty.Application.Interfaces.Messages.UserLikedMessages;
using Shifty.Common.Exceptions;
using Shifty.Domain.Messages.UserLikedMessages;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Messages.Commands.LikeMessages;

public class LikeMessageCommandHandler(
    IdentityService service,
    IUserLikedMessagesQueryRepository userLikedMessagesQueryRepository,
    IUserLikedMessageCommandRepository userLikedMessageCommandRepository,
    ILogger<LikeMessageCommandHandler> logger,
    IStringLocalizer<LikeMessageCommandHandler> localizer
) : IRequestHandler<LikeMessageCommand>
{
    public async Task Handle(LikeMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null)
                throw new InvalidNullInputException(nameof(request));

            var userId = service.GetUserId<Guid>();

            var userLikeMessage =
                await userLikedMessagesQueryRepository.GetUserLikedMessageAsync(request.MessageId,
                    userId,
                    cancellationToken);

            if (userLikeMessage is null)
            {
                var like = request.Adapt<UserLikedMessage>();
                like.MessageId = request.MessageId;
                like.UserId = userId;

                await userLikedMessageCommandRepository.AddAsync(like, cancellationToken);
                logger.LogInformation("User {UserId} liked message {MessageId}.", userId, request.MessageId);
            }
            else
            {
                await userLikedMessageCommandRepository.DeleteAsync(userLikeMessage, cancellationToken);
                logger.LogInformation("User {UserId} unliked message {MessageId}.", userId, request.MessageId);
            }
        }
        catch (ShiftyException ex)
        {
            logger.LogError(ex, "Business error occurred while updating like status.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while updating like status.");
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while updating the like status."]);
        }
    }
}