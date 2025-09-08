using SmartAttendance.Application.Features.Messages.Commands.VisitMessage;
using SmartAttendance.Application.Features.Messages.Queries.GetMessageById;
using SmartAttendance.Application.Features.Messages.Request.Queries.GetMessageById;
using SmartAttendance.Application.Interfaces.Messages;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Messages.Queries.GetMessageById;

public class GetMessageByIdQueryHandler(
    IMessageQueryRepository messageQueryRepository,
    IMediator mediator,
    IdentityService identityService,
    ILogger<GetMessageByIdQueryHandler> logger,
    IStringLocalizer<GetMessageByIdQueryHandler> localizer
)
    : IRequestHandler<GetMessageByIdQuery, GetMessageByIdResponse>
{
    public async Task<GetMessageByIdResponse> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var userId = identityService.GetUserId<Guid>();


            if (!identityService.GetRoles().Contains(Roles.Messages_Read))
            {
                logger.LogWarning("User {UserId} doesn't have global ReadMessages role.", userId);
                throw SmartAttendanceException.Forbidden(localizer["You do not have the necessary role to read messages."]);
            }


            var message = await messageQueryRepository.GetMessagesById(request.Id, cancellationToken);

            if (message is null)
            {
                logger.LogWarning("Message with ID {MessageId} not found.", request.Id);
                throw SmartAttendanceException.NotFound(localizer["Message not found."]);
            }

            await mediator.Send(new VisitMessageCommand(message.Id), cancellationToken);
            logger.LogInformation("User {UserId} viewed message {MessageId}.", userId, message.Id);

            return message;
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business exception occurred while fetching message {MessageId}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while fetching message {MessageId}.", request.Id);

            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while retrieving the message."]);
        }
    }
}