namespace Shifty.Common.Common.ESBase;

public record DomainEvent : IDomainEvent
{
    public DateTime Reported { get; init; }
    public DateTime EventTime { get; init; } = DateTime.Now;
    public Guid ProjectId { get; init; }
}