using Shifty.Application.Interfaces.Messages.MessageTargetUsers;
using Shifty.Domain.Messages.MessageTargetUsers;

namespace Shifty.Persistence.Repositories.Messages.MessageTargetUsers;

public class MessageTargetUsersQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<MessageTargetUser>> logger
)
    : QueryRepository<MessageTargetUser>(dbContext, logger),
        IMessageTargetUsersQueryRepository
{
}