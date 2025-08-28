using Shifty.Application.Features.Messages.Comments.Request.Queries.GetComments;
using Shifty.Application.Features.Messages.Request.Commands.CreateMessage;
using Shifty.Application.Features.Messages.Request.Queries.GetMessage;
using Shifty.Application.Features.Messages.Request.Queries.GetMessageById;
using Shifty.Application.Interfaces.Messages;
using Shifty.Common.Common.Responses.Users.Queries.Base;
using Shifty.Common.General.Enums;
using Shifty.Common.General.Enums.Access;
using Shifty.Common.General.Enums.Projects;
using Shifty.Common.Utilities.TypeConverters;
using Shifty.Domain.Messages;

namespace Shifty.Persistence.Repositories.Messages;

public class MessageQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Message>> logger,
    IMediator mediator,
    IStringLocalizer<MessageQueryRepository> localizer,
    IdentityService service,
    IUserQueryRepository userQueryRepository
)
    : QueryRepository<Message>(dbContext, logger),
        IMessageQueryRepository
{
    public async Task<List<GetMessageResponse>> GetMessagesAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = service.GetUserId<Guid>();

            logger.LogInformation("Fetching project messages for UserId: {UserId}",
                userId);

            var predicate = await BuildMessageFilter(userId);


            var messages = await TableNoTracking.Where(a => a.IsActive)
                .Where(predicate)
                .Include(m => m.UserVisitedMessages)
                .Include(m => m.UserLikedMessages)
                .Include(m => m.UserTargetMessages)
                .Include(m => m.Comments)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(cancellationToken);

            if (!messages.Any())
            {
                logger.LogInformation("No messages found.");
                return new List<GetMessageResponse>();
            }

            var recipientIds = messages
                .Where(m => m.CreatedBy == userId)
                .SelectMany(m => m.UserTargetMessages.Select(t => t.UserId))
                .Distinct()
                .ToList();

            var userResponse = await userQueryRepository.TableNoTracking
                .Where(u => recipientIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.FullName(), cancellationToken);

            var response = messages
                .Where(a => a.IsActive)
                .Select(m => new GetMessageResponse
                {
                    Id = m.Id,
                    Title = m.Title,
                    User = service.GetFullName(),
                    Description = m.Description,
                    ImageUrl = m.ImageUrl?.BuildImageUrl(true),
                    ImageCompressed = m.ImageUrl?.BuildImageUrl(true),
                    CreatedAt = m.CreatedAt,
                    Visit = m.UserVisitedMessages.Count(l => l.MessageId == m.Id),
                    IsLiked = m.UserLikedMessages.Any(l => l.UserId == userId),
                    Like = m.UserLikedMessages.Count(l => l.MessageId == m.Id),
                    CommentCount = m.Comments.Count,
                    Seen = m.UserVisitedMessages.Any(uvm => uvm.UserId == userId),
                    UserId = m.CreatedBy,
                    Recipients = m.CreatedBy == userId
                        ? m.UserTargetMessages.Select(mtu =>
                            {
                                userResponse.TryGetValue(mtu.UserId, out var fullName);
                                return new UserMessageResponse
                                {
                                    Id = mtu.UserId,
                                    Name = fullName ?? string.Empty
                                };
                            })
                            .ToList()
                        : null!
                })
                .ToList();

            logger.LogInformation("Fetched {Count} messages", response.Count);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching messages");

            throw ShiftyException.InternalServerError(
                additionalData: localizer["An error occurred while retrieving messages."]);
        }
    }


    public Task<bool> CanUserPerformDelete(Message message, Guid userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(message.CreatedBy == userId);
    }

    public async Task<GetMessageByIdResponse?> GetMessagesById(Guid id, CancellationToken cancellationToken)
    {
        var userId = service.GetUserId<Guid>();

        try
        {
            logger.LogInformation("Retrieving message details. MessageId: {MessageId}, UserId: {UserId}", id, userId);

            var entity = await TableNoTracking.Where(a => a.IsActive)
                .Include(a => a.Comments)
                .Include(a => a.UserVisitedMessages)
                .Include(a => a.UserLikedMessages)
                .Include(a => a.UserTargetMessages)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (entity is null)
            {
                logger.LogWarning("Message not found. MessageId: {MessageId}", id);
                return null;
            }

            if (entity.CreatedBy != userId)
            {
                logger.LogWarning("Access denied. User {UserId} is not authorized to view Message {MessageId}",
                    userId,
                    entity.Id);

                throw ShiftyException.Forbidden(localizer["You are not authorized to view this message."]);
            }


            var recipientUserIds = entity.CreatedBy == userId
                ? entity.UserTargetMessages.Where(mtu => mtu.MessageId == entity.Id).Select(mtu => mtu.UserId).ToList()
                : new List<Guid>();

            var commentUserIds = entity.Comments
                .Where(c => c.IsActive && c.CreatedBy.HasValue)
                .Select(c => c.CreatedBy!.Value)
                .Distinct()
                .ToList();

            var allUserIds = recipientUserIds
                .Union(commentUserIds)
                .Distinct()
                .ToList();

            Dictionary<Guid, string> userNames = new();

            if (allUserIds.Count != 0)
                try
                {
                    var response = await userQueryRepository.TableNoTracking
                        .Where(u => allUserIds.Contains(u.Id))
                        .ToDictionaryAsync(u => u.Id, u => u.FullName());

                    userNames = response;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Error occurred while retrieving user names via message broker.");
                }


            var message = new GetMessageByIdResponse
            {
                Id = entity.Id,
                Title = entity.Title,
                User = service.GetFullName(),
                Description = entity.Description,
                ImageUrl = entity.ImageUrl!.BuildImageUrl(),
                ImageCompressed = entity.ImageUrl!.BuildImageUrl(true),
                CreatedAt = entity.CreatedAt,
                Visit = entity.UserVisitedMessages.Count(l => l.MessageId == entity.Id),
                IsLiked = entity.UserLikedMessages.Any(l => l.UserId == userId),
                Like = entity.UserLikedMessages.Count(l => l.MessageId == entity.Id),
                CommentCount = entity.Comments.Count(c => c.IsActive),
                Seen = entity.UserVisitedMessages.Any(uvm => uvm.UserId == userId),
                UserId = entity.CreatedBy,
                Recipients = entity.CreatedBy == userId
                    ? entity.UserTargetMessages.Where(mtu => mtu.MessageId == entity.Id)
                        .Select(mtu => new UserMessageResponse
                        {
                            Id = mtu.UserId,
                            Name = userNames.TryGetValue(mtu.UserId, out var name) ? name : ""
                        })
                        .ToList()
                    : null!,
                Comments = entity.Comments.Where(c => c.IsActive)
                    .Select(c => new GetCommentResponse
                    {
                        Id = c.Id,
                        CreatedAt = c.CreatedAt,
                        Author = c.CreatedBy.HasValue && userNames.TryGetValue(c.CreatedBy.Value, out var name)
                            ? name
                            : string.Empty,
                        Text = c.Text,
                        ParentId = c.RelatedCommentId
                    })
                    .ToList()
            };

            logger.LogInformation("Message retrieved successfully. MessageId: {MessageId}", id);
            return message;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving message. MessageId: {MessageId}", id);

            throw ShiftyException.InternalServerError(
                additionalData: localizer["An error occurred while retrieving the message."]);
        }
    }

    private async Task<Expression<Func<Message, bool>>> BuildMessageFilter(
        Guid userId)
    {
        var roles = service.GetRoles();

        if (roles.Contains(Roles.Admin) ||
            roles.Contains(Roles.Messages_Edit) ||
            roles.Contains(Roles.ManageMessages) ||
            roles.Contains(Roles.Messages_Delete) ||
            roles.Contains(Roles.Messages_Read) ||
            roles.Contains(Roles.Messages_Create))
            return m =>
                m.CreatedBy == userId || m.UserTargetMessages.Any(utm => utm.UserId == userId);

        return m => false;
    }
}