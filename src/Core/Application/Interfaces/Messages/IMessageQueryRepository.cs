using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Features.Messages.Request.Queries.GetMessage;
using Shifty.Application.Features.Messages.Request.Queries.GetMessageById;
using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Messages;

namespace Shifty.Application.Interfaces.Messages;

public interface IMessageQueryRepository : IQueryRepository<Message>,
    IScopedDependency
{
    Task<List<GetMessageResponse>> GetMessagesAsync(
        CancellationToken cancellationToken);

    Task<bool> CanUserPerformDelete(Message message, Guid userId, CancellationToken cancellationToken);

    Task<GetMessageByIdResponse?> GetMessagesById(Guid id, CancellationToken cancellationToken);
}