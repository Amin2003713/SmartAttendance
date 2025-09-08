using SmartAttendance.Application.Interfaces.Messages.UserLikedMessages;
using SmartAttendance.Domain.Messages.UserLikedMessages;

namespace SmartAttendance.Persistence.Repositories.Messages.UserLikedMessages;

public class UserLikedMessagesQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<UserLikedMessage>> logger,
    IStringLocalizer<UserLikedMessagesQueryRepository> localizer
)
    : QueryRepository<UserLikedMessage>(dbContext, logger),
        IUserLikedMessagesQueryRepository
{
    public async Task<UserLikedMessage?> GetUserLikedMessageAsync(
        Guid messageId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking liked message for UserId: {UserId} and MessageId: {MessageId}",
                userId,
                messageId);

            var result = await TableNoTracking.Where(a => a.UserId == userId && a.MessageId == messageId)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                logger.LogInformation("No liked message found for UserId: {UserId} and MessageId: {MessageId}",
                    userId,
                    messageId);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error checking liked message for UserId: {UserId}, MessageId: {MessageId}",
                userId,
                messageId);

            throw new InvalidOperationException(localizer["An error occurred while retrieving liked message."].Value);
        }
    }
}