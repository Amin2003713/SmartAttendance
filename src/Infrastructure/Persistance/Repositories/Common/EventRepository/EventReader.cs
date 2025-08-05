using System.Text.Json;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Utilities.MongoHelpers;

namespace Shifty.Persistence.Repositories.Common.EventRepository;

/// <summary>
///     MongoDB implementation of IEventReader for reading event-sourced aggregates and snapshots.
///     Supports hybrid rehydration: loads latest snapshot then applies subsequent events.
/// </summary>
public class EventReader<TAggregate, TId>(
    IMongoDatabase database
) : IEventReader<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>, new() where TId : notnull
{
    // Maps event name to CLR Type
    private readonly static IReadOnlyDictionary<string, Type> _eventTypeMap = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t => typeof(IDomainEvent).IsAssignableFrom(t) &&
                    !t.IsInterface &&
                    !t.IsAbstract)
        .ToDictionary(t => t.Name, t => t);

    // Maps snapshot name to CLR Type
    private readonly static IReadOnlyDictionary<string, Type> _snapshotTypeMap = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t => t.GetInterfaces()
                        .Any(i => i.IsGenericType &&
                                  i.GetGenericTypeDefinition() ==
                                  typeof(ISnapshot<>)) &&
                    !t.IsInterface &&
                    !t.IsAbstract)
        .ToDictionary(t => t.Name, t => t);

    private IMongoCollection<EventDocument<TId>> Events => database.GetCollection<EventDocument<TId>>(
        ApplicationConstant.Mongo.GetEventsCollectionName(typeof(TAggregate).Name));

    private IMongoCollection<SnapShotDocument<TId>> Snapshots => database.GetCollection<SnapShotDocument<TId>>(
        ApplicationConstant.Mongo.GetSnapShotCollectionName(typeof(TAggregate).Name));


    /// <inheritdoc />
    public async Task<TAggregate?> LoadAsync(TId aggregateId, CancellationToken cancellationToken = default)
    {
        return (await LoadHybridAsync(model => model.AggregateId.Equals(aggregateId),
            cancellationToken: cancellationToken)).SingleOrDefault();
    }

    public async Task<SnapShotDocument<TId>> LoadSnapShotAsync(
        TId aggregateId,
        CancellationToken cancellationToken = default)
    {
        return await Snapshots.Find(s => s.AggregateId.Equals(aggregateId))
            .SortByDescending(s => s.Version)
            .Limit(1)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(TId aggregateId, CancellationToken cancellationToken = default)
    {
        return await Events.Find(e => e.AggregateId.Equals(aggregateId)).AnyAsync(cancellationToken);
    }

    public async Task<bool> SnapShotExistsAsync(TId aggregateId, CancellationToken cancellationToken = default)
    {
        return await Snapshots.Find(e => e.AggregateId.Equals(aggregateId)).AnyAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<IDomainEvent>> LoadEventsAsync(
        TId aggregateId,
        CancellationToken cancellationToken = default)
    {
        var docs = await Events.Find(e => e.AggregateId.Equals(aggregateId))
            .SortBy(e => e.Version)
            .ToListAsync(cancellationToken);

        return docs.Select(DeserializeEvent).ToList();
    }

    public async Task<List<EventDocument<TId>>> LoadEventsAsync(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var opts = new AggregateOptions
        {
            AllowDiskUse = true
        };

        var eventFilter = AdaptExpression<BaseEventStoreModel<TId>, EventDocument<TId>>(predicate);


        var docs = await Events.Aggregate(opts)
            .Match(eventFilter)
            .SortBy(e => e.Version)
            .ToListAsync(cancellationToken);

        return docs.ToList();
    }

    /// <inheritdoc />
    public async Task<long> GetLatestVersionAsync(TId aggregateId, CancellationToken cancellationToken = default)
    {
        var latest = await Events.Find(e => e.AggregateId.Equals(aggregateId))
            .SortByDescending(e => e.Version)
            .Limit(1)
            .FirstOrDefaultAsync(cancellationToken);

        return latest?.Version ?? -1;
    }


    /// <inheritdoc />
    public async Task<List<TAggregate>> LoadByPredicateAsync(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate = null!,
        CancellationToken cancellationToken = default)
    {
        return await LoadHybridAsync(predicate, cancellationToken: cancellationToken);
    }


    /// <inheritdoc />
    public async Task<bool> AnyAsync(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate = null!,
        CancellationToken cancellationToken = default)
    {
        return (await LoadHybridAsync(predicate, cancellationToken: cancellationToken)).Any();
    }

    /// <inheritdoc />
    public async Task<bool> Any(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate = null!,
        CancellationToken cancellationToken = default)
    {
        return (await LoadHybridAsync(predicate, cancellationToken: cancellationToken)).Any();
    }

    /// <inheritdoc />
    public async ValueTask<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return (await LoadHybridAsync(model => model.AggregateId.Equals(id), cancellationToken: cancellationToken))
            .SingleOrDefault();
    }


    /// <inheritdoc />
    public async Task<TAggregate?> GetSingleAsync(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate = null!,
        CancellationToken cancellationToken = default)
    {
        return (await LoadHybridAsync(predicate, cancellationToken: cancellationToken)).SingleOrDefault();
    }

    public async Task<Guid?> GetSnapShotIdAsync(
        Expression<Func<SnapShotDocument<TId>, bool>> predicate = null!,
        CancellationToken cancellationToken = default)
    {
        return (await GetSnapShot(predicate, cancellationToken))?.Id ?? null!;
    }


    /// <summary>
    ///     بارگذاری ترکیبی (Hybrid) آaggregate با استفاده از اسنپ‌شات‌ها و رویدادها از MongoDB.
    ///     این متد ابتدا اسنپ‌شات‌های مربوط به هر aggregate را بازیابی می‌کند،
    ///     سپس رویدادهای مربوط به هر آaggregate را دریافت و بازپخش (replay) می‌کند،
    ///     و در نهایت بر اساس دسترسی (access) فیلتر کرده و نتیجه را مرتب می‌کند.
    /// </summary>
    /// <typeparam name="TAggregate">نوع Aggregate که باید بارگذاری شود.</typeparam>
    /// <typeparam name="TId">نوع شناسه (ID) Aggregate (مثلاً Guid).</typeparam>
    /// <param name="predicate">
    ///     معیار (Predicate) برای فیلتر کردن اسنپ‌شات‌ها و رویدادها.
    ///     این عبارت باید از نوع Expression بر روی مدل ذخیره‌سازی رویداد/اسنپ‌شات باشد.
    ///     اگر مقدار null باشد، هیچ فیلتری اعمال نمی‌شود.
    /// </param>
    /// <param name="paginate"></param>
    /// <param name="access">
    ///     اطلاعات دسترسی کاربر (شامل سطح دسترسی و شناسه کاربر).
    ///     اگر null باشد، همه Aggregates بارگذاری می‌شوند.
    /// </param>
    /// <param name="cancellationToken">توکنی برای لغو عملیات به صورت همزمان (cancellation token).</param>
    /// <returns>
    ///     فهرستی از شیءهای Aggregate بارگذاری‌شده و فیلترشده مطابق با دسترسی کاربر.
    ///     نتیجه بر اساس زمان آخرین فعالیت (LastAction) به صورت نزولی مرتب شده است.
    /// </returns>
    public async Task<List<TAggregate>> LoadHybridAsync(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate = null!,
        Func<OrderedParallelQuery<TAggregate>, List<TAggregate>> paginate = null!,
        CancellationToken cancellationToken = default)
    {
        var opts = new AggregateOptions
        {
            AllowDiskUse = true
        };

        // 1) Build MongoDB filters
        var eventFilter    = AdaptExpression<BaseEventStoreModel<TId>, EventDocument<TId>>(predicate);
        var snapshotFilter = AdaptExpression<BaseEventStoreModel<TId>, SnapShotDocument<TId>>(predicate);

        // 2) Load snapshots
        var snapshotGroups = await Snapshots.Aggregate(opts)
            .Match(snapshotFilter)
            .Group(
                s => s.AggregateId,
                g => new
                {
                    AggregateId = g.Key,
                    LatestSnapshot = g.OrderByDescending(x => x.Version).FirstOrDefault()
                }
            )
            .ToListAsync(cancellationToken);

        // 3) Load event groups
        var eventGroups = await Events.Aggregate(opts)
            .Match(eventFilter)
            .Group(
                e => e.AggregateId,
                g => new
                {
                    AggregateId = g.Key,
                    AllEvents = g.OrderBy(x => x.Version).ToList()
                }
            )
            .ToListAsync(cancellationToken);

        var eventsById = eventGroups.ToDictionary(x => x.AggregateId, x => x.AllEvents);

        // var isSuperUser = access?.AccessList.HasAccess((int)TenantAccess.Admin) == true;

        var aggregates = snapshotGroups.AsParallel()
            .WithCancellation(cancellationToken)
            .Where(sg =>
            {
                // if (access is not { Node: UserType.Contractor } || isSuperUser)
                //     return true;

                return eventsById.TryGetValue(sg.AggregateId, out var evs) &&
                       evs.Any(e => e.Version == 0
                           // && e.UserId == access.UserId
                       );
            })
            .Select(sg =>
            {
                var aggregate = new TAggregate(); // new instance

                var snapDoc = sg.LatestSnapshot;

                if (snapDoc is not null &&
                    _snapshotTypeMap.TryGetValue(snapDoc.Type, out var snapType))
                {
                    var snapshot = (ISnapshot<TId>)JsonSerializer.Deserialize(
                        snapDoc.Data,
                        snapType,
                        ApplicationConstant.Mongo.JsonOptions)!;

                    aggregate.LoadFromSnapshot(snapshot);
                }

                if (!eventsById.TryGetValue(sg.AggregateId, out var rawEvents))
                    return aggregate;

                var domainEvents = rawEvents
                    .Where(e => snapDoc == null || e.Version > snapDoc.Version)
                    .Select(DeserializeEvent)
                    .ToList();

                if (domainEvents.Count > 0)
                    aggregate.LoadFromHistory(domainEvents);

                if (aggregate.Deleted)
                    return null!;

                aggregate.LoadEvents(rawEvents
                    .ToList());

                return aggregate;

                // return access switch
                //        {
                //            null                                                                => aggregate,
                //            { Node: UserType.Stakeholder } when aggregate.Node != UserType.None => aggregate,
                //            { Node: UserType.Contractor } when aggregate.Node == UserType.None  => aggregate,
                //            _                                                                   => aggregate.Node >= access.Node ? aggregate : null!
                //        };
            })
            .Where(a => a != null)
            .OrderByDescending(a => a.LastAction);

        return paginate != null ? paginate(aggregates) : aggregates.ToList();
    }


    public async Task<List<TResult>> LoadStateByPredicateAsync<TResult>(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return (await LoadByPredicateAsync(predicate, cancellationToken)).Adapt<List<TResult>>();
    }

    /// <inheritdoc />
    public async Task<TAggregate> FirstOrDefaultsAsync(
        Expression<Func<BaseEventStoreModel<TId>, bool>> predicate = null!,
        CancellationToken cancellationToken = default)
    {
        return (await LoadHybridAsync(predicate, cancellationToken: cancellationToken)).FirstOrDefault()!;
    }


    public async Task<SnapShotDocument<TId>?> GetSnapShot(
        Expression<Func<SnapShotDocument<TId>, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await Snapshots.Find(predicate).FirstOrDefaultAsync(cancellationToken);
    }


    private static IDomainEvent DeserializeEvent(EventDocument<TId> doc)
    {
        if (!_eventTypeMap.TryGetValue(doc.Type, out var type))
            throw new InvalidOperationException($"Unknown event type '{doc.Type}'.");

        return (IDomainEvent)JsonSerializer.Deserialize(doc.Data, type, ApplicationConstant.Mongo.JsonOptions)!;
    }

    public static IDomainEvent DeserializeEvent(string typeName, string json)
    {
        return DeserializeEvent(new EventDocument<TId>
        {
            Type = typeName,
            Data = json
        });
    }

    private static Expression<Func<TDerived, bool>> AdaptExpression<TBase, TDerived>(Expression<Func<TBase, bool>> baseExpr)
        where TDerived : TBase
    {
        // create a new parameter whose Type is TDerived
        var derivedParam = Expression.Parameter(typeof(TDerived), baseExpr.Parameters[0].Name);
        // rewrite the body, replacing occurrences of the old param with the new
        var newBody = new ReplaceParameterVisitor(baseExpr.Parameters[0], derivedParam).Visit(baseExpr.Body);
        // re-lambda that body over the TDerived parameter
        return Expression.Lambda<Func<TDerived, bool>>(newBody, derivedParam);
    }
}