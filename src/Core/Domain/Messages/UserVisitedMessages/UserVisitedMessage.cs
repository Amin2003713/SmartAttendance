using Shifty.Common.General.BaseClasses;

namespace Shifty.Domain.Messages.UserVisitedMessages;

public class UserVisitedMessage : BaseEntity
{
    public Guid MessageId { get; set; }
    public Message Message { get; set; }
    public Guid UserId { get; set; }
}