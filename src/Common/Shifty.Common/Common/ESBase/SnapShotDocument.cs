using Shifty.Common.General.Enums;

namespace Shifty.Common.Common.ESBase;

public class SnapShotDocument<TId> : BaseEventStoreModel<TId>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public TId AggregateId { get; set; } = default!;
    public long Version { get; set; }
    public string Type { get; set; } = null!;
    public string Data { get; set; } = null!;
    public DateTime OccurredOn { get; set; }
    public DateTime Reported { get; set; }
    public Guid UserId { get; set; }
    public UserType Node { get; set; }
}