using SmartAttendance.Application.Features.Attachments.Responses;
using SmartAttendance.Common.General.Enums.Excuse;
using SmartAttendance.Domain.Features.Excuses;

namespace SmartAttendance.Application.Features.Excuses.Responses;

public class GetExcuseInfoResponse
{
    public ExcuseStatus Status { get; set; }
    public DateTime SubmittedAt { get; set; }
    public GetAttachmentInfoResponse Attachment { get; set; }
}