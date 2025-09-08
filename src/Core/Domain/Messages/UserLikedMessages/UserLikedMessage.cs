using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Messages.UserLikedMessages;

public class UserLikedMessage : BaseEntity
{
    public Guid MessageId { get; set; }

    public Message Message { get; set; }

    public Guid UserId { get; set; }
}