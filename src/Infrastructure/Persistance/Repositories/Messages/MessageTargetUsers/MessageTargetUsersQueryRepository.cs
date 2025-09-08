using SmartAttendance.Application.Interfaces.Messages.MessageTargetUsers;
using SmartAttendance.Domain.Messages.MessageTargetUsers;

namespace SmartAttendance.Persistence.Repositories.Messages.MessageTargetUsers;

public class MessageTargetUsersQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<MessageTargetUser>> logger
)
    : QueryRepository<MessageTargetUser>(dbContext, logger),
        IMessageTargetUsersQueryRepository { }