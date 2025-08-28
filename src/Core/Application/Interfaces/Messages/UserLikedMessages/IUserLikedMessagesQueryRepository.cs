using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Messages.UserLikedMessages;

namespace Shifty.Application.Interfaces.Messages.UserLikedMessages;

public interface IUserLikedMessagesQueryRepository : IQueryRepository<UserLikedMessage>,
    IScopedDependency
{
    Task<UserLikedMessage?> GetUserLikedMessageAsync(Guid messageId, Guid userId, CancellationToken cancellationToken);
}