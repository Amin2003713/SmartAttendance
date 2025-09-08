using SmartAttendance.Application.Interfaces.Messages.UserVisitedMessages;
using SmartAttendance.Domain.Messages.UserVisitedMessages;

namespace SmartAttendance.Persistence.Repositories.Messages.UserVisitedMessages;

public class UserVisitedMessageQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<UserVisitedMessage>> logger
)
    : QueryRepository<UserVisitedMessage>(dbContext, logger),
        IUserVisitedMessageQueryRepository
{
    public async Task<UserVisitedMessage?> GetUserLikedMessageAsync(
        Guid messageId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        {
            return await TableNoTracking.Where(a => a.UserId == userId)
                .Where(a => a.MessageId == messageId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}