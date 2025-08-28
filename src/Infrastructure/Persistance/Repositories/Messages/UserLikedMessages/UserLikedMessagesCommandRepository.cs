using Shifty.Application.Interfaces.Messages.UserLikedMessages;
using Shifty.Domain.Messages.UserLikedMessages;

namespace Shifty.Persistence.Repositories.Messages.UserLikedMessages;

public class UserLikedMessagesCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<UserLikedMessage>> logger
)
    : CommandRepository<UserLikedMessage>(dbContext, logger),
        IUserLikedMessageCommandRepository
{
}