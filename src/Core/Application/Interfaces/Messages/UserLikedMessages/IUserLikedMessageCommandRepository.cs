using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Messages.UserLikedMessages;

namespace Shifty.Application.Interfaces.Messages.UserLikedMessages;

public interface IUserLikedMessageCommandRepository : ICommandRepository<UserLikedMessage>,
    IScopedDependency
{
}