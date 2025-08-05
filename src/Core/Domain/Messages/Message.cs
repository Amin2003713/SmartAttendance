using Shifty.Common.General.BaseClasses;
using Shifty.Domain.Messages.Comments;
using Shifty.Domain.Messages.MessageTargetUsers;
using Shifty.Domain.Messages.UserLikedMessages;
using Shifty.Domain.Messages.UserVisitedMessages;

namespace Shifty.Domain.Messages;

public class Message : BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public int Visit { get; set; }

    public int Like { get; set; }

    // public Guid? ProjectId { get; set; }

    public ICollection<Comment> Comments { get; set; } = [];

    public virtual List<UserVisitedMessage> UserVisitedMessages { get; set; } = [];
    public virtual List<MessageTargetUser> UserTargetMessages { get; set; } = [];

    public virtual List<UserLikedMessage> UserLikedMessages { get; set; } = [];

    public void Update(Message adapt)
    {
        Title = adapt.Title;
        Description = adapt.Description;
        ImageUrl = adapt.ImageUrl;
        UserTargetMessages = adapt.UserTargetMessages;
    }
}