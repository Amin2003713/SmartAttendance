using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Messages.UserLikedMessages;

namespace SmartAttendance.Application.Interfaces.Messages.UserLikedMessages;

public interface IUserLikedMessagesQueryRepository : IQueryRepository<UserLikedMessage>,
    IScopedDependency
{
    Task<UserLikedMessage?> GetUserLikedMessageAsync(Guid messageId, Guid userId, CancellationToken cancellationToken);
}