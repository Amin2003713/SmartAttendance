using Shifty.Application.Features.Messages.Request.Queries.GetMessage;

namespace Shifty.Application.Features.Messages.Queries.GetMessage;

public record GetMessageQuery : IRequest<List<GetMessageResponse>>;