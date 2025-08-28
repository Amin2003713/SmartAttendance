// using System.Threading;
// using System.Threading.Tasks;
// using Shifty.Common.Common.ESBase;
//
// namespace Shifty.Application.Interfaces.Base.EventInterface;
//
// /// <summary>
// ///     Interface for writing event-sourced aggregates to the event store.
// ///     Supports advanced operations like bulk insert, update, and delete.
// /// </summary>
// public interface IEventWriter<TAggregate, TId>
//     where TAggregate : AggregateRoot<TId>
// {
//     /// <summary>
//     ///     Saves all uncommitted events and optionally snapshots of the aggregate to the store.
//     /// </summary>
//     /// <example>
//     ///     <code>
//     /// var contractor = ContractorES.Create(...);
//     /// contractor.UpdateDetails(...);
//     /// await writer.SaveAsync(contractor);
//     /// </code>
//     /// </example>
//     Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
//
//
//     /// <summary>
//     ///     Saves all uncommitted events and optionally snapshots of the aggregate(s) to the store.
//     /// </summary>
//     /// <example>
//     ///     <code>
//     /// await writer.SaveAsync(contractors , cancellationToken);
//     /// </code>
//     /// </example>
//     Task SaveAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken = default);
//
//     /// <summary>
//     ///     Deletes all persisted events and snapshots for the specified aggregate.
//     /// </summary>
//     /// <example>
//     ///     <code>
//     /// await writer.DeleteAggregateAsync(contractor.Id);
//     /// </code>
//     /// </example>
//     Task DeleteAggregateAsync(TId aggregateId, CancellationToken cancellationToken = default);
//
//     /// <summary>
//     ///     Stores a snapshot for an aggregate manually.
//     /// </summary>
//     /// <example>
//     ///     <code>
//     /// var snapshot = contractor.GetSnapshot();
//     /// await writer.SaveSnapshotAsync(snapshot);
//     /// </code>
//     /// </example>
//     Task SaveSnapshotAsync(
//         ISnapshot<TId> snapshot,
//         DateTime reported,
//         CancellationToken cancellationToken = default);
//
//     /// <summary>
//     ///     Clears all events and snapshots across all aggregates of this type (dangerous operation).
//     /// </summary>
//     /// <example>
//     ///     <code>
//     /// await writer.TruncateAllAsync("IUnderstand");
//     /// </code>
//     /// </example>
//     Task TruncateAllAsync(string confirmation, CancellationToken cancellationToken = default);
// }