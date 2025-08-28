using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Messages.MessageTargetUsers;

namespace Shifty.Application.Interfaces.Messages.MessageTargetUsers;

public interface IMessageTargetUsersQueryRepository : IQueryRepository<MessageTargetUser>,
    IScopedDependency
{
}