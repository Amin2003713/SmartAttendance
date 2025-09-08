using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Messages.Comments.Queries.GetComments;
using SmartAttendance.Application.Features.Messages.Comments.Request.Queries.GetComments;
using SmartAttendance.Application.Interfaces.Messages;
using SmartAttendance.Application.Interfaces.Users;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Messages.Comments.Queries.GetComments;

public class GetCommentQueryHandler(
    IMessageQueryRepository messageQueryRepository,
    IUserQueryRepository userQueryRepository,
    ILogger<GetCommentQueryHandler> logger,
    IStringLocalizer<GetCommentQueryHandler> localizer
) : IRequestHandler<GetCommentQuery, List<GetCommentResponse>>
{
    public async Task<List<GetCommentResponse>> Handle(
        GetCommentQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching comments for TargetId: {TargetId}", request.Id);

            var message = await messageQueryRepository
                .GetSingleAsync(cancellationToken, x => x.Id == request.Id);

            await messageQueryRepository
                .LoadCollectionAsync(message, m => m.Comments, cancellationToken);

            var authorIds = message.Comments
                .Select(c => c.CreatedBy)
                .Distinct()
                .ToList();

            var authorResponse = await userQueryRepository.TableNoTracking
                .Where(u => authorIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.FullName(), cancellationToken);

            var result = message.Comments
                .Select(c =>
                {
                    var a = authorResponse.FirstOrDefault(x => x.Key == c.CreatedBy);
                    return new GetCommentResponse
                    {
                        Id = c.Id,
                        Text = c.Text,
                        CreatedAt = c.CreatedAt,
                        Author = a.Value
                    };
                })
                .ToList();

            logger.LogInformation(
                "Retrieved {Count} comments for TargetId: {TargetId}",
                result.Count,
                request.Id);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving comments for TargetId: {TargetId}", request.Id);
            throw SmartAttendanceException.InternalServerError(localizer["Failed to retrieve comments."]);
        }
    }
}