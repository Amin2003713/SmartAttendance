using SmartAttendance.Application.Features.Attachments.Responses;
using SmartAttendance.Common.General.Enums.Excuse;

namespace SmartAttendance.Application.Features.Excuses.Responses;

public class GetExcuseInfoResponse
{
    public Guid Id { get; set; }
    public ExcuseStatus Status { get; set; }
    public DateTime SubmittedAt { get; set; }
    public GetAttachmentInfoResponse? Attachment { get; set; }
    public string Reason { get ; set ; }
}