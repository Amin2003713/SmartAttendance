using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Messages.UserVisitedMessages;

public class UserVisitedMessage : BaseEntity
{
    public Guid MessageId { get; set; }
    public Message Message { get; set; }
    public Guid UserId { get; set; }
}