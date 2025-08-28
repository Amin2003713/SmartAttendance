using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Messages.UserVisitedMessages;

namespace Shifty.Application.Interfaces.Messages.UserVisitedMessages;

public interface IUserVisitedMessageCommandRepository : ICommandRepository<UserVisitedMessage>,
    IScopedDependency
{
}