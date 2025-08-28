using Shifty.Application.Features.Messages.Queries.GetMessage;
using Shifty.Application.Features.Messages.Request.Queries.GetMessage;
using Shifty.Application.Interfaces.Messages;
using Shifty.Common.Exceptions;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Messages.Queries.GetMessage;

public class GetMessageQueryHandler(
    IMessageQueryRepository messageQueryRepository,
    IdentityService service,
    ILogger<GetMessageQueryHandler> logger,
    IStringLocalizer<GetMessageQueryHandler> localizer
)
    : IRequestHandler<GetMessageQuery, List<GetMessageResponse>>
{
    public async Task<List<GetMessageResponse>> Handle(
        GetMessageQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var userId = service.GetUserId<Guid>();

            var messages = await messageQueryRepository.GetMessagesAsync(cancellationToken);

            logger.LogInformation("User {UserId} retrieved {Count} messages for project {ProjectId}.",
                userId,
                messages.Count,
                request);

            return messages;
        }
        catch (ShiftyException ex)
        {
            logger.LogError(ex, "Business error occurred while retrieving project messages.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while retrieving project messages.");
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while retrieving messages."]);
        }
    }
}