using Shifty.Application.Features.Messages.Request.Queries.GetMessageById;

namespace Shifty.Application.Features.Messages.Queries.GetMessageById;

public record GetMessageByIdQuery(
    Guid Id
) : IRequest<GetMessageByIdResponse>;