using SmartAttendance.Application.Features.Messages.Commands.DeleteMassage;
using SmartAttendance.Application.Interfaces.Messages;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Messages.Commands.DeleteMessage;

public class DeleteMessageCommandHandler(
    IdentityService service,
    IMessageCommandRepository commandRepository,
    IMessageQueryRepository queryRepository,
    ILogger<DeleteMessageCommandHandler> logger,
    IStringLocalizer<DeleteMessageCommandHandler> localizer
) : IRequestHandler<DeleteMessageCommand>
{
    public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var message = await queryRepository.GetByIdAsync(cancellationToken, request.MessageId);

            if (message == null)
            {
                logger.LogWarning("Message with ID {MessageId} not found.", request.MessageId);
                throw SmartAttendanceException.NotFound(localizer["Message not found."]);
            }

            var userId = service.GetUserId<Guid>();

            if (!await queryRepository.CanUserPerformDelete(message, userId, cancellationToken))
            {
                logger.LogWarning("User {UserId} not allowed to delete message {MessageId}.",
                    userId,
                    request.MessageId);

                throw SmartAttendanceException.Forbidden(localizer["You are not allowed to delete this message."]);
            }

            await commandRepository.DeleteAsync(message, cancellationToken);
            logger.LogInformation("Message {MessageId} deleted by user {UserId}.", request.MessageId, userId);
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business exception occurred while deleting message.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while deleting message.");
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while deleting the message."]);
        }
    }
}