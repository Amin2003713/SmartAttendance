using Shifty.Common.Common.Responses.ItemNamesBaseResponse;
using Shifty.Common.General.Enums;

namespace Shifty.Common.Common.ESBase;

/// <summary>
///     Base class for event-sourced aggregates with generic ID type.
///     Supports event application, versioning, and snapshotting.
/// </summary>
/// <typeparam name="TId">Type of the aggregate ID (e.g., long, Guid)</typeparam>
public abstract class AggregateRoot<TId>
{
    private readonly List<IDomainEvent> _uncommittedEvents = new();
    public List<EventDocument<TId>> StoredEvents { get; protected set; } = [];

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


    public ReviewAction ReviewAction { get; protected set; } = ReviewAction.New;

    public DateTime LastAction { get; protected set; }

    public bool Deleted { get; protected set; }

    public SortedList<DateTime, DataReviewLog> ReviewEvents { get; protected set; } = [];


    public IReadOnlyDictionary<UserType, ReviewAction> ReviewStatus { get; private set; }
        = new Dictionary<UserType, ReviewAction>();

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
        GetPerLevelReviewStatus();
    }

    public void LoadEvents(List<EventDocument<TId>> document)
    {
        StoredEvents.AddRange(document);
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

        GetPerLevelReviewStatus();
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
    ///     Returns a read‐only dictionary that maps each level (1 .. Node) to its ReviewAction.
    ///     If there is at least one DataReviewLog whose PerformedByLevel == level,
    ///     we take the most‐recent log (highest DataReviewedAt) and use its Action (Verify/Reject).
    ///     Otherwise, that level is still “New.”
    ///     Also assigns the computed dictionary into the private property PerLevelReviewStatus.
    /// </summary>
    public void GetPerLevelReviewStatus()
    {
        if (ReviewEvents.Count == 0)
            return;

        // 1) Determine up to which level we need to report.
        //    Node is an enum: UserType.None = 0, Contractor = 1, Manager = 2, Stakeholder = 3, etc.
        var maxLevel = (int)ReviewEvents.Reverse().FirstOrDefault().Value.PerformedByLevel;

        // 2) Group all ReviewEvents by (int)PerformedByLevel.
        //    Then pick the single‐most‐recent (largest DataReviewedAt) log per level.
        var mostRecentLogPerLevel = ReviewEvents.Values.GroupBy(log => (int)log.PerformedByLevel)
            .ToDictionary(
                grp => grp.Key,
                grp => grp.OrderByDescending(x => x.DataReviewedAt).First()
            );

        // 3) Build a Dictionary<UserType, ReviewAction> for levels 1..maxLevel.
        var result = new Dictionary<UserType, ReviewAction>(maxLevel);

        for (var level = 1; level <= maxLevel; level++)
        {
            var levelEnum = (UserType)level;

            if (mostRecentLogPerLevel.TryGetValue(level, out var lastLog))
            {
                switch (lastLog.Action)
                {
                    case ReviewAction.Reject:
                        result[levelEnum - 1] = lastLog.Action;
                        break;
                    case ReviewAction.Verify:
                        result[levelEnum + 1] = ReviewAction.New;
                        break;
                }

                result[levelEnum] = lastLog.Action;
            }
            else
            {
                // Otherwise, this level has had no logs → it is still “New.”
                result[levelEnum] = ReviewAction.New;
            }
        }

        // 4) Assign into the private property, then return it.
        ReviewStatus = result.Where(node => node.Key <= Node).ToDictionary();
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