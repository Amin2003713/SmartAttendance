namespace Shifty.Common.Common.ESBase;

/// <summary>
///     Represents a snapshot of an aggregate's state at a certain version.
/// </summary>
/// <typeparam name="TId">Type of the aggregate ID.</typeparam>
public interface ISnapshot<TId>
{
    /// <summary>
    ///     Unique identifier of the aggregate the snapshot belongs to.
    /// </summary>
    TId AggregateId { get; init; }

    /// <summary>
    ///     Version of the aggregate at the time of snapshot.
    /// </summary>
    int Version { get; init; }

    Guid ProjectId { get; init; }
    DateTime LastAction { get; init; }
}