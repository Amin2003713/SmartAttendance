using SmartAttendance.Application.Interfaces.Messages.Comments;
using SmartAttendance.Domain.Messages.Comments;

namespace SmartAttendance.Persistence.Repositories.Messages.Comments;

public class CommentQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Comment>> logger
)
    : QueryRepository<Comment>(dbContext, logger),
        ICommentQueryRepository { }