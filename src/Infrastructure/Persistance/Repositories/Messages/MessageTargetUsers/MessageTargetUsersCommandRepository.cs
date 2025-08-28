using Shifty.Application.Interfaces.Messages.MessageTargetUsers;
using Shifty.Domain.Messages.MessageTargetUsers;

namespace Shifty.Persistence.Repositories.Messages.MessageTargetUsers;

public class MessageTargetUsersCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<MessageTargetUser>> logger
)
    : CommandRepository<MessageTargetUser>(dbContext, logger),
        IMessageTargetUsersCommandRepository
{
}