using Microsoft.AspNetCore.Http;

namespace SmartAttendance.Application.Features.Attachments.Commands.Create;

public class CreateAttachmentCommand  : IRequest
{
    public IFormFile File { get; set; }
    public Guid RowId { get ; set ; }
}