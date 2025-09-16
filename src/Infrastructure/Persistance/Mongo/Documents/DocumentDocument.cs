namespace SmartAttendance.Persistence.Mongo.Documents;

public sealed class DocumentDocument
{
	public Guid Id { get; set; }
	public string FileName { get; set; } = string.Empty;
	public string ContentType { get; set; } = string.Empty;
	public long SizeBytes { get; set; }
	public DateTime UploadedAtUtc { get; set; }
	public Guid? AttendanceId { get; set; }
	public int Status { get; set; }
}

