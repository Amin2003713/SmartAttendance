using SmartAttendance.Application.Features.Messages.Request.Queries.GetMessage;

namespace SmartAttendance.Application.Features.Messages.Queries.GetMessage;

public record GetMessageQuery : IRequest<List<GetMessageResponse>>;