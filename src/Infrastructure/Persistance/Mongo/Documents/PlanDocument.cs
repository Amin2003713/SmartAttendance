namespace SmartAttendance.Persistence.Mongo.Documents;

public sealed class PlanDocument
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartsAtUtc { get; set; }
    public DateTime EndsAtUtc { get; set; }
    public int Capacity { get; set; }
    public int Reserved { get; set; }
    public List<Guid> Students { get; set; } = new();
    public List<Guid> WaitingList { get; set; } = new();
}