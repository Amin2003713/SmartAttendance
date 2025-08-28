using Shifty.Application.Interfaces.Messages.Comments;
using Shifty.Domain.Messages.Comments;

namespace Shifty.Persistence.Repositories.Messages.Comments;

public class CommentCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Comment>> logger
)
    : CommandRepository<Comment>(dbContext, logger),
        ICommentCommandRepository
{
}