using SmartAttendance.Application.Interfaces.Messages.MessageTargetUsers;
using SmartAttendance.Domain.Messages.MessageTargetUsers;

namespace SmartAttendance.Persistence.Repositories.Messages.MessageTargetUsers;

public class MessageTargetUsersCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<MessageTargetUser>> logger
)
    : CommandRepository<MessageTargetUser>(dbContext, logger),
        IMessageTargetUsersCommandRepository { }