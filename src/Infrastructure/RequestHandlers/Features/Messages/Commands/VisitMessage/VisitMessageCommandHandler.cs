using Mapster;
using Shifty.Application.Features.Messages.Commands.VisitMessage;
using Shifty.Application.Interfaces.Messages.UserVisitedMessages;
using Shifty.Common.Exceptions;
using Shifty.Domain.Messages.UserVisitedMessages;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Messages.Commands.VisitMessage;

public class VisitMessageCommandHandler(
    IdentityService service,
    IUserVisitedMessageQueryRepository userVisitedMessageQueryRepository,
    IUserVisitedMessageCommandRepository userVisitedMessageCommandRepository,
    ILogger<VisitMessageCommandHandler> logger,
    IStringLocalizer<VisitMessageCommandHandler> localizer
) : IRequestHandler<VisitMessageCommand>
{
    public async Task Handle(VisitMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var userId = service.GetUserId<Guid>();

            var visitMessage =
                await userVisitedMessageQueryRepository.GetUserLikedMessageAsync(request.Id, userId, cancellationToken);

            if (visitMessage is null)
            {
                var visit = request.Adapt<UserVisitedMessage>();
                visit.UserId = userId;
                visit.MessageId = request.Id;
                visit.CreatedBy = userId;

                await userVisitedMessageCommandRepository.AddAsync(visit, cancellationToken);

                logger.LogInformation("User {UserId} visited message {MessageId}.", userId, request.Id);
            }
            else
            {
                logger.LogInformation("User {UserId} has already visited message {MessageId}. Skipping insert.",
                    userId,
                    request.Id);
            }
        }
        catch (ShiftyException ex)
        {
            logger.LogError(ex, "Business exception occurred while visiting message.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while visiting message.");
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while visiting the message."]);
        }
    }
}