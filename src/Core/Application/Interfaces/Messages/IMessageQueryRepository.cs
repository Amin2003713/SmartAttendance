using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Features.Messages.Request.Queries.GetMessage;
using SmartAttendance.Application.Features.Messages.Request.Queries.GetMessageById;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Messages;

namespace SmartAttendance.Application.Interfaces.Messages;

public interface IMessageQueryRepository : IQueryRepository<Message>,
    IScopedDependency
{
    Task<List<GetMessageResponse>> GetMessagesAsync(CancellationToken cancellationToken);

    Task<bool> CanUserPerformDelete(Message message, Guid userId, CancellationToken cancellationToken);

    Task<GetMessageByIdResponse?> GetMessagesById(Guid id, CancellationToken cancellationToken);
}