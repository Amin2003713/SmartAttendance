using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Messages;

namespace Shifty.Application.Interfaces.Messages;

public interface IMessageCommandRepository : ICommandRepository<Message>,
    IScopedDependency
{
}