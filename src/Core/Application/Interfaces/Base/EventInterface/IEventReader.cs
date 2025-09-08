using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Common.Common.ESBase;

namespace SmartAttendance.Application.Interfaces.Base.EventInterface;

/// <summary>
///     Interface for reading event-sourced aggregates from the event store.
/// </summary>
public interface IEventReader<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>, new()
{
    /// <summary>
    ///     Loads and rehydrates an aggregate by its unique identifier.
    /// </summary>
    /// <example>
    ///     <code>
    /// var contractor = await reader.LoadAsync(contractorId);
    /// Console.WriteLine(contractor.ProjectId);
    /// </code>
    /// </example>
    Task<TAggregate?> LoadAsync(TId aggregateId, CancellationToken cancellationToken = default);


    /// <summary>
    ///     Determines whether any events exist for the specified aggregate.
    /// </summary>
    /// <example>
    ///     <code>
    /// bool exists = await reader.ExistsAsync(contractorId);
    /// </code>
    /// </example>
    Task<bool> ExistsAsync(TId aggregateId, CancellationToken cancellationToken = default);


    /// <summary>
    ///     Loads all domain events associated with the specified aggregate.
    /// </summary>
    /// <example>
    ///     <code>
    /// var events = await reader.LoadEventsAsync(contractorId);
    /// foreach (var e in events) Console.WriteLine(e.OccurredOn);
    /// </code>
    /// </example>
    Task<List<IDomainEvent>> LoadEventsAsync(TId aggregateId, CancellationToken cancellationToken = default);

    Task<List<EventDocument<TId>>> LoadEventsAsync(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves the most recent version of an aggregate.
    /// </summary>
    /// <example>
    ///     <code>
    /// int version = await reader.GetLatestVersionAsync(contractorId);
    /// </code>
    /// </example>
    Task<long> GetLatestVersionAsync(TId aggregateId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Loads aggregates where at least one event matches the provided predicate.
    /// </summary>
    /// <example>
    ///     <code>
    /// var aggregates = await reader.LoadByPredicateAsync(e => e.ProjectId == myProjectId);
    /// </code>
    /// </example>
    Task<List<TAggregate>> LoadByPredicateAsync(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate,
        CancellationToken cancellationToken = default);


    Task<List<TResult>> LoadStateByPredicateAsync<TResult>(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate,
        CancellationToken cancellationToken = default);


    /// <summary>
    ///     Retrieves exactly one aggregate matching the predicate or throws if not found or multiple results found.
    /// </summary>
    /// <example>
    ///     <code>
    /// var single = await reader.GetSingleAsync(predicate: x => x.ProjectId == id);
    /// </code>
    /// </example>
    Task<TAggregate?> GetSingleAsync(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate = null!,
        CancellationToken cancellationToken = default);


    /// <summary>
    ///     Checks if any aggregates match the specified predicate.
    /// </summary>
    /// <example>
    ///     <code>
    /// bool any = await reader.AnyAsync(x => x.ProjectId == id);
    /// </code>
    /// </example>
    Task<bool> AnyAsync(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate,
        CancellationToken cancellationToken = default);


    /// <summary>
    ///     Retrieves an aggregate by ID. Useful in scenarios like reference loading.
    /// </summary>
    /// <example>
    ///     <code>
    /// var contractor = await reader.GetByIdAsync(id);
    /// </code>
    /// </example>
    ValueTask<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<List<TAggregate>> LoadHybridAsync(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate = null,
        Func<OrderedParallelQuery<TAggregate>, List<TAggregate>> paginate = null,
        CancellationToken cancellationToken = default);
}