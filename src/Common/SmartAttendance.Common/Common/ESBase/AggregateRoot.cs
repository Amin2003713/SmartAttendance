using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Common.Common.ESBase;

/// <summary>
///     Base class for event-sourced aggregates with generic ID type.
///     Supports event application, versioning, and snapshotting.
/// </summary>
/// <typeparam name="TId">Type of the aggregate ID (e.g., long, Guid)</typeparam>
public abstract class AggregateRoot<TId>
{
    private readonly List<IDomainEvent> _uncommittedEvents = new List<IDomainEvent>();

    /// <summary>
    ///     Aggregate identifier.
    /// </summary>
    public TId AggregateId { get; protected set; } = default!;

    /// <summary>
    ///     Current version of the aggregate (incremented per applied event).
    ///     Starts at -1 when no events are applied.
    /// </summary>
    public int Version { get; protected set; } = -1;

    public UserType Node { get; protected set; }


    public DateTime LastAction { get; protected set; }

    public bool Deleted { get; protected set; }


    /// <summary>
    ///     Gets all uncommitted domain events.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> GetUncommittedEvents()
    {
        return _uncommittedEvents.AsReadOnly();
    }

    /// <summary>
    ///     Clears all uncommitted events after persistence.
    /// </summary>
    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }

    /// <summary>
    ///     Raises and applies a new domain event (write path).
    /// </summary>
    /// <param name="event">The domain event to raise and apply.</param>
    protected void RaiseEvent(IDomainEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ApplyChange(@event, true);
    }

    /// <summary>
    ///     Applies historical events to rehydrate the aggregate (read path).
    /// </summary>
    /// <param name="history">Domain events to replay.</param>
    public void LoadFromHistory(IEnumerable<IDomainEvent> history)
    {
        ArgumentNullException.ThrowIfNull(history);

        foreach (var @event in history)
        {
            ApplyChange(@event, false);
        }

        ClearUncommittedEvents();
    }


    /// <summary>
    ///     Restores the aggregate from a snapshot and then applies subsequent events.
    /// </summary>
    /// <param name="snapshot">Snapshot object.</param>
    /// <param name="events">Events occurred after the snapshot.</param>
    public void LoadFromSnapshot(ISnapshot<TId> snapshot = null!, IEnumerable<IDomainEvent> events = null!)
    {
        if (snapshot is not null)
        {
            RestoreFromSnapshot(snapshot);
            Version = snapshot.Version;
        }


        if (events is not null)
            LoadFromHistory(events);

    }

    /// <summary>
    ///     Applies a domain event to the aggregate.
    ///     Uses dynamic dispatch to invoke the appropriate Apply(...) method.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    /// <param name="isNew">Whether the event is new or from history.</param>
    private void ApplyChange(IDomainEvent @event, bool isNew)
    {
        ((dynamic)this).Apply((dynamic)@event);

        if (isNew) _uncommittedEvents.Add(@event);

        Version++;
        LastAction = @event.EventTime;
    }



    /// <summary>
    ///     Restores the aggregate's state from a snapshot.
    ///     Override this in the derived class.
    /// </summary>
    /// <param name="snapshot">Snapshot to restore from.</param>
    protected virtual void RestoreFromSnapshot(ISnapshot<TId> snapshot)
    {
        throw new NotImplementedException($"{GetType().Name} has not implemented snapshot restoration.");
    }

    /// <summary>
    ///     Exports the current state of the aggregate as a snapshot.
    ///     Override this in the derived class.
    /// </summary>
    /// <returns>Snapshot object representing current state.</returns>
    public virtual ISnapshot<TId> GetSnapshot()
    {
        throw new NotImplementedException($"{GetType().Name} has not implemented snapshot export.");
    }
}