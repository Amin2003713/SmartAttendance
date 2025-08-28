using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Messages.UserVisitedMessages;

namespace Shifty.Application.Interfaces.Messages.UserVisitedMessages;

public interface IUserVisitedMessageQueryRepository : IQueryRepository<UserVisitedMessage>,
    IScopedDependency
{
    Task<UserVisitedMessage?>
        GetUserLikedMessageAsync(Guid messageId, Guid userId, CancellationToken cancellationToken);
}