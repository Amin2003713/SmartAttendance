namespace SmartAttendance.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}