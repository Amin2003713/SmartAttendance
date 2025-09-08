namespace SmartAttendance.Domain.Tenants;

public class TenantRequest
{
    public Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);
    public DateTime RequestTime { get; set; }
    public string Endpoint { get; set; }
    public string TenantId { get; set; }
    public Guid? UserId { get; set; } = null!;
    public string CorrelationId { get; set; }

    public string ServiceName { get; set; }
}