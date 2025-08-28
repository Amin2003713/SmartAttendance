using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Messages.Comments;

namespace Shifty.Application.Interfaces.Messages.Comments;

public interface ICommentCommandRepository : ICommandRepository<Comment>,
    IScopedDependency
{
}