using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Domain.Messages.Comments;
using SmartAttendance.Domain.Messages.MessageTargetUsers;
using SmartAttendance.Domain.Messages.UserLikedMessages;
using SmartAttendance.Domain.Messages.UserVisitedMessages;

namespace SmartAttendance.Domain.Messages;

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