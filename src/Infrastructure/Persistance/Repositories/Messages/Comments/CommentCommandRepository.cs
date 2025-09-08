using SmartAttendance.Application.Interfaces.Messages.Comments;
using SmartAttendance.Domain.Messages.Comments;

namespace SmartAttendance.Persistence.Repositories.Messages.Comments;

public class CommentCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Comment>> logger
)
    : CommandRepository<Comment>(dbContext, logger),
        ICommentCommandRepository { }