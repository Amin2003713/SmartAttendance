using SmartAttendance.Application.Features.Messages.Comments.Request.Queries.GetComments;

namespace SmartAttendance.Application.Features.Messages.Comments.Queries.GetComments;

public record GetCommentQuery(
    Guid Id
) : IRequest<List<GetCommentResponse>>;