using SmartAttendance.Application.Features.Messages.Request.Queries.GetMessageById;

namespace SmartAttendance.Application.Features.Messages.Queries.GetMessageById;

public record GetMessageByIdQuery(
    Guid Id
) : IRequest<GetMessageByIdResponse>;