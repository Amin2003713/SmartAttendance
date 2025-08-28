using Shifty.Application.Interfaces.Messages.Comments;
using Shifty.Domain.Messages.Comments;

namespace Shifty.Persistence.Repositories.Messages.Comments;

public class CommentQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Comment>> logger
)
    : QueryRepository<Comment>(dbContext, logger),
        ICommentQueryRepository
{
}