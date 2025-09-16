namespace SmartAttendance.Application.Features.Documents.Responses;

// DTO نمایش سند
public sealed class DocumentDto
{
    public Guid Id { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public long SizeBytes { get; init; }
    public DateTime UploadedAtUtc { get; init; }
    public Guid? AttendanceId { get; init; }
    public string Status { get; init; } = string.Empty;
}