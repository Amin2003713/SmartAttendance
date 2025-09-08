using SmartAttendance.Application.Interfaces.Messages;
using SmartAttendance.Domain.Messages;

namespace SmartAttendance.Persistence.Repositories.Messages;

public class MessageCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Message>> logger
)
    : CommandRepository<Message>(dbContext, logger),
        IMessageCommandRepository { }