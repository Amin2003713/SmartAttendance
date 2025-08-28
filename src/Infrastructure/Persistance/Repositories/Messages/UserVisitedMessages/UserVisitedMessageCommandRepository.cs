using Shifty.Application.Interfaces.Messages.UserVisitedMessages;
using Shifty.Domain.Messages.UserVisitedMessages;

namespace Shifty.Persistence.Repositories.Messages.UserVisitedMessages;

public class UserVisitedMessageCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<UserVisitedMessage>> logger
)
    : CommandRepository<UserVisitedMessage>(dbContext, logger),
        IUserVisitedMessageCommandRepository
{
}