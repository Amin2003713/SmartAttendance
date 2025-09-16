using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.NotificationAggregate;

// موجودیت اعلان دامنه (بدون ارسال زیرساختی)
public sealed class Notification : Entity<NotificationId>
{
    public Notification(NotificationId id, UserId recipientId, string title, string message, NotificationChannel channel)
        : base(id)
    {
        RecipientId = recipientId;
        Title = string.IsNullOrWhiteSpace(title) ? throw new DomainValidationException("عنوان اعلان الزامی است.") : title.Trim();
        Message = string.IsNullOrWhiteSpace(message) ? throw new DomainValidationException("متن اعلان الزامی است.") : message.Trim();
        Channel = channel is NotificationChannel.Unknown ? throw new DomainValidationException("کانال اعلان نامعتبر است.") : channel;
    }

    public UserId RecipientId { get; }
    public string Title { get; }
    public string Message { get; }
    public NotificationChannel Channel { get; }
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
    public bool IsSent { get; private set; }
    public bool IsRead { get; private set; }

    public void MarkAsSent()
    {
        IsSent = true;
    }

    // علامت‌گذاری به عنوان خوانده شده
    public void MarkAsRead()
    {
        if (IsRead) return;

        IsRead = true;
    }
}