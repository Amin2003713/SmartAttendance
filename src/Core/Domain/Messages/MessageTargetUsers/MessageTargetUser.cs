namespace Shifty.Domain.Messages.MessageTargetUsers;

public class MessageTargetUser : BaseEntity
{
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
    public Message Message { get; set; }
}