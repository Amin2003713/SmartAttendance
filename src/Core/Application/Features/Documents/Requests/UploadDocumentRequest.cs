namespace SmartAttendance.Application.Features.Documents.Requests;

// درخواست آپلود سند (فقط متادیتا در لایه دامنه)
public sealed class UploadDocumentRequest
{
	public string FileName { get; init; } = string.Empty;
	public string ContentType { get; init; } = string.Empty;
	public long SizeBytes { get; init; }
}

