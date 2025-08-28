using Shifty.Application.Features.Messages.Comments.Request.Queries.GetComments;

namespace Shifty.Application.Features.Messages.Comments.Queries.GetComments;

public record GetCommentQuery(
    Guid Id
) : IRequest<List<GetCommentResponse>>;