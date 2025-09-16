using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.DocumentAggregate;

// موجودیت سند جهت نگهداری متادیتا و جریان تایید/رد
public sealed class Document : Entity<DocumentId>
{
	public string FileName { get; }
	public string ContentType { get; }
	public long SizeBytes { get; }
	public DateTime UploadedAtUtc { get; }
	public AttendanceId? AttendanceId { get; }
	public DocumentStatus Status { get; private set; } = DocumentStatus.Uploaded;

	public Document(DocumentId id, string fileName, string contentType, long sizeBytes, DateTime? uploadedAtUtc = null, AttendanceId? attendanceId = null)
		: base(id)
	{
		FileName = string.IsNullOrWhiteSpace(fileName) ? throw new DomainValidationException("نام فایل الزامی است.") : fileName.Trim();
		ContentType = string.IsNullOrWhiteSpace(contentType) ? throw new DomainValidationException("نوع محتوا الزامی است.") : contentType.Trim();
		if (sizeBytes <= 0) throw new DomainValidationException("حجم فایل نامعتبر است.");
		SizeBytes = sizeBytes;
		UploadedAtUtc = uploadedAtUtc ?? DateTime.UtcNow;
		AttendanceId = attendanceId;
	}

	// تایید سند
	public void Approve()
	{
		if (Status == DocumentStatus.Approved) return;
		Status = DocumentStatus.Approved;
	}

	// رد سند
	public void Reject()
	{
		if (Status == DocumentStatus.Rejected) return;
		Status = DocumentStatus.Rejected;
	}
}

public enum DocumentStatus
{
	Uploaded = 0,
	Approved = 1,
	Rejected = 2
}

