using Shifty.Application.Interfaces.Messages;
using Shifty.Domain.Messages;

namespace Shifty.Persistence.Repositories.Messages;

public class MessageCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Message>> logger
)
    : CommandRepository<Message>(dbContext, logger),
        IMessageCommandRepository
{
}