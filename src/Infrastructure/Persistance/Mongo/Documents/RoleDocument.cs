namespace SmartAttendance.Persistence.Mongo.Documents;

public sealed class RoleDocument
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}