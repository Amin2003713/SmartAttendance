using SmartAttendance.Application.Interfaces.Messages.UserVisitedMessages;
using SmartAttendance.Domain.Messages.UserVisitedMessages;

namespace SmartAttendance.Persistence.Repositories.Messages.UserVisitedMessages;

public class UserVisitedMessageCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<UserVisitedMessage>> logger
)
    : CommandRepository<UserVisitedMessage>(dbContext, logger),
        IUserVisitedMessageCommandRepository { }