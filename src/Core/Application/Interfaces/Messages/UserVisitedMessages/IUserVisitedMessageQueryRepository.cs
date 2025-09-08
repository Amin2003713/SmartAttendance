using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Messages.UserVisitedMessages;

namespace SmartAttendance.Application.Interfaces.Messages.UserVisitedMessages;

public interface IUserVisitedMessageQueryRepository : IQueryRepository<UserVisitedMessage>,
    IScopedDependency
{
    Task<UserVisitedMessage?>
        GetUserLikedMessageAsync(Guid messageId, Guid userId, CancellationToken cancellationToken);
}