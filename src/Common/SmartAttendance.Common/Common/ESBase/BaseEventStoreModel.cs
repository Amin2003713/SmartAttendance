namespace SmartAttendance.Common.Common.ESBase;

public interface BaseEventStoreModel<TId>
{
    public Guid Id { get; set; }
    public TId AggregateId { get; set; }
    public long Version { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTime OccurredOn { get; set; }
    public DateTime Reported { get; set; }
    public Guid UserId { get; set; }
}