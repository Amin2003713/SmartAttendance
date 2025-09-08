using SmartAttendance.Application.Interfaces.Messages.UserLikedMessages;
using SmartAttendance.Domain.Messages.UserLikedMessages;

namespace SmartAttendance.Persistence.Repositories.Messages.UserLikedMessages;

public class UserLikedMessagesCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<UserLikedMessage>> logger
)
    : CommandRepository<UserLikedMessage>(dbContext, logger),
        IUserLikedMessageCommandRepository { }