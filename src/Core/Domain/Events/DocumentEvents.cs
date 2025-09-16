using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

// رویدادهای مربوط به اسناد
public sealed record DocumentApprovedEvent(DocumentId DocumentId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record DocumentRejectedEvent(DocumentId DocumentId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

