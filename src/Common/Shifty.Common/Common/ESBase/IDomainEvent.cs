namespace Shifty.Common.Common.ESBase;

/// <summary>
///     Represents a single event that modifies the state of an aggregate.
/// </summary>
public interface IDomainEvent
{
    DateTime Reported { get; init; }
    public DateTime EventTime { get; init; }
    Guid ProjectId { get; init; }
}