using System.Text.Json;
using Shifty.Application.Interfaces.Base.EventInterface;

namespace Shifty.Persistence.Repositories.Common.EventRepository;

/// <summary>
///     MongoDB implementation of IEventWriter for writing events and snapshots.
///     Uses type-safe serialization and avoids reliance on type names.
///     Includes runtime-safe deserialization via type name mapping.
/// </summary>
public class EventWriter<TAggregate, TId> : IEventWriter<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>, new() where TId : notnull
{
    private readonly IMongoDatabase                _database;
    private readonly IEventReader<TAggregate, TId> _eventReader;
    private readonly IdentityService               _identityService;

    /// <summary>
    ///     MongoDB implementation of IEventWriter for writing events and snapshots.
    ///     Uses type-safe serialization and avoids reliance on type names.
    ///     Includes runtime-safe deserialization via type name mapping.
    /// </summary>
    public EventWriter(
        IMongoDatabase database,
        IdentityService identityService,
        IEventReader<TAggregate, TId> eventReader)
    {
        _database = database;
        _identityService = identityService;

        _eventReader = eventReader;

        // — single-field on AggregateId —
        var aggIdIndex = new CreateIndexModel<EventDocument<TId>>(
            Builders<EventDocument<TId>>.IndexKeys.Ascending(e => e.AggregateId),
            new CreateIndexOptions
            {
                Name = "idx_events_AggregateId"
            }
        );

        // — compound on AggregateId + Version —
        var aggIdVersionIndex = new CreateIndexModel<EventDocument<TId>>(
            Builders<EventDocument<TId>>.IndexKeys.Ascending(e => e.AggregateId).Ascending(e => e.Version),
            new CreateIndexOptions
            {
                Name = "idx_events_AggregateId_Version"
            }
        );


        var dataIndex = new CreateIndexModel<EventDocument<TId>>(
            Builders<EventDocument<TId>>.IndexKeys.Text(e => e.Data),
            new CreateIndexOptions
            {
                Name = "idx_data_search"
            }
        );

        // — your existing indexes —
        var compoundEventIndex = new CreateIndexModel<EventDocument<TId>>(
            Builders<EventDocument<TId>>.IndexKeys.Ascending(e => e.AggregateId)
                .Ascending(e => e.Version)
                .Descending(e => e.OccurredOn),
            new CreateIndexOptions
            {
                Name = "idx_events_AggId_Version_OccurredOn"
            }
        );

        var occurredOnIndex = new CreateIndexModel<EventDocument<TId>>(
            Builders<EventDocument<TId>>.IndexKeys.Ascending(e => e.OccurredOn),
            new CreateIndexOptions
            {
                Name = "idx_events_OccurredOn"
            }
        );

        // create them all in one go
        Events.Indexes.CreateMany(new[]
        {
            aggIdIndex,
            aggIdVersionIndex,
            compoundEventIndex,
            occurredOnIndex,
            dataIndex
        });

        // — and if you want the same for snapshots —
        var snapAggIdIndex = new CreateIndexModel<SnapShotDocument<TId>>(
            Builders<SnapShotDocument<TId>>.IndexKeys.Ascending(s => s.AggregateId),
            new CreateIndexOptions
            {
                Name = "idx_snapshots_AggregateId"
            }
        );

        var snapAggIdVersionIndex = new CreateIndexModel<SnapShotDocument<TId>>(
            Builders<SnapShotDocument<TId>>.IndexKeys.Ascending(s => s.AggregateId).Ascending(s => s.Version),
            new CreateIndexOptions
            {
                Name = "idx_snapshots_AggregateId_Version"
            }
        );

        SnapShots.Indexes.CreateMany(new[]
        {
            snapAggIdIndex,
            snapAggIdVersionIndex
        });
    }


    private IMongoCollection<EventDocument<TId>> Events => _database.GetCollection<EventDocument<TId>>(
        ApplicationConstant.Mongo.GetEventsCollectionName(typeof(TAggregate).Name));

    private IMongoCollection<SnapShotDocument<TId>> SnapShots => _database.GetCollection<SnapShotDocument<TId>>(
        ApplicationConstant.Mongo.GetSnapShotCollectionName(typeof(TAggregate).Name));


    public async Task SaveAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken = default)
    {
        var list = aggregates?.ToList();
        if (list == null || list.Count == 0) return;


        // Check concurrency in parallel
        var versionTasks = list.ToDictionary(
            agg => agg,
            agg => _eventReader.GetLatestVersionAsync(agg.AggregateId, cancellationToken)
        );

        await Task.WhenAll(versionTasks.Values);

        foreach (var kv in versionTasks.Where(kv => kv.Value.Result >= kv.Key.Version))
        {
            throw new InvalidOperationException(
                $"Concurrency conflict for {kv.Key.AggregateId}.");
        }

        // Build event documents
        var docs = list.SelectMany(agg =>
            {
                // use Result to get the completed version task
                var version = versionTasks[agg].Result + 1;

                return agg.GetUncommittedEvents()
                    .Select(evt => new EventDocument<TId>
                    {
                        Id = Guid.CreateVersion7(DateTimeOffset.UtcNow),
                        AggregateId = agg.AggregateId,
                        Version = version++,
                        Type = evt.GetType().Name!,
                        Data = SerializeEvent(evt),
                        OccurredOn = DateTime.UtcNow,
                        // UserId = access.UserId,

                        Reported = evt.Reported
                        // Node = access.Node
                    });
            })
            .ToList();

        if (docs.Count > 0)
            await Events.InsertManyAsync(docs, cancellationToken: cancellationToken);

        // Build and save snapshots in bulk
        var snapshots = list.Where(agg => agg.Version % 2 == 0 || agg.Version == 0)
            .Select(agg =>
            {
                var snap     = agg.GetSnapshot();
                var reported = agg.GetUncommittedEvents().First().Reported;

                return new SnapShotDocument<TId>
                {
                    Id = Guid.CreateVersion7(),
                    AggregateId = snap.AggregateId,
                    Version = snap.Version,
                    Type = snap.GetType().Name!,
                    Data = JsonSerializer.Serialize(
                        snap,
                        snap.GetType(),
                        ApplicationConstant.Mongo.JsonOptions),
                    OccurredOn = DateTime.UtcNow,

                    Reported = reported
                    //   Node = access.Node,
                    // UserId = access.UserId
                };
            })
            .ToList();

        if (snapshots.Count > 0)
            await SnapShots.InsertManyAsync(snapshots, cancellationToken: cancellationToken);

        // Clear uncommitted events
        list.ForEach(agg => agg.ClearUncommittedEvents());
    }

    public async Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        // var access =
        //     await _messageBroker.RequestAsync<GetProjectUserAccessBrokerResponse, GetProjectUserAccessBroker>(
        //         new GetProjectUserAccessBroker(aggregate.ProjectId, _identityService.GetUserId<Guid>()),
        //         cancellationToken);
        //
        // if (access is null)
        //     throw IpaException.Forbidden("forbid");


        var latestVersion = await _eventReader.GetLatestVersionAsync(aggregate.AggregateId, cancellationToken);

        if (latestVersion >= aggregate.Version)
            throw new InvalidOperationException("Concurrency conflict: aggregate version mismatch.");

        await AppendEventsAsync(aggregate.GetUncommittedEvents(), aggregate.AggregateId, cancellationToken);

        if (aggregate.Version % 2 == 0 || aggregate.Version == 0)
            await SaveSnapshotAsync(aggregate.GetSnapshot(),
                aggregate.GetUncommittedEvents().FirstOrDefault()!.Reported,
                // access,
                cancellationToken);

        aggregate.ClearUncommittedEvents();
    }

    public async Task DeleteAggregateAsync(TId aggregateId, CancellationToken cancellationToken = default)
    {
        await Events.DeleteManyAsync(x => x.AggregateId!.Equals(aggregateId), cancellationToken);
        await SnapShots.DeleteManyAsync(x => x.AggregateId!.Equals(aggregateId), cancellationToken);
    }

    public async Task SaveSnapshotAsync(
        ISnapshot<TId> snapshot,
        DateTime reported,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        // 1) Build your full snapshot document, 
        var doc = new SnapShotDocument<TId>
        {
            Id = Guid.CreateVersion7(),
            AggregateId = snapshot.AggregateId,
            Version = snapshot.Version,
            Type = snapshot.GetType().Name!,
            Data = JsonSerializer.Serialize(
                snapshot,
                snapshot.GetType(),
                ApplicationConstant.Mongo.JsonOptions),
            OccurredOn = DateTime.UtcNow,

            Reported = reported
            // Node = access.Node,
            // UserId = access.UserId
        };

        await SnapShots.InsertOneAsync(doc, cancellationToken: cancellationToken);
    }


    public async Task TruncateAllAsync(string confirmation, CancellationToken cancellationToken = default)
    {
        if (confirmation != typeof(TAggregate).Name)
            throw new InvalidOperationException("Confirmation required for truncation");

        await Events.DeleteManyAsync(_ => true, cancellationToken);
        await SnapShots.DeleteManyAsync(_ => true, cancellationToken);
    }

    public async Task AppendEventsAsync(
        IEnumerable<IDomainEvent> events,
        TId aggregateId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(events);

        var startVersion = await _eventReader.GetLatestVersionAsync(aggregateId, cancellationToken) + 1;

        var docs = events.Select((e, _) => new EventDocument<TId>
            {
                Id = Guid.CreateVersion7(DateTimeOffset.Now),
                AggregateId = aggregateId,
                Version = startVersion++,
                Type = e.GetType().Name!,
                Data = SerializeEvent(e),
                OccurredOn = DateTime.UtcNow,
                // UserId = access.UserId,

                Reported = e.Reported
                // Node = access.Node
            })
            .ToList();

        await Events.InsertManyAsync(docs, null!, cancellationToken);
    }

    private static string SerializeEvent(IDomainEvent domainEvent)
    {
        return JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), ApplicationConstant.Mongo.JsonOptions);
    }
}