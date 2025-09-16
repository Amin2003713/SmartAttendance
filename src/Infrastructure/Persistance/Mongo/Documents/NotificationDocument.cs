namespace SmartAttendance.Persistence.Mongo.Documents;

public sealed class NotificationDocument
{
	public Guid Id { get; set; }
	public Guid RecipientId { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	public string Channel { get; set; } = "Unknown";
	public bool IsSent { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}

