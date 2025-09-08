namespace SmartAttendance.Common.Common.ESBase;

public record DomainEvent : IDomainEvent
{
    public DateTime Reported { get; init; }
    public DateTime EventTime { get; init; } = DateTime.UtcNow;
}