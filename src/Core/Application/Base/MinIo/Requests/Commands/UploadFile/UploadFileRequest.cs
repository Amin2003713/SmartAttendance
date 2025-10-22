using Microsoft.AspNetCore.Http;
using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Application.Base.MinIo.Requests.Commands.UploadFile;

public class UploadFileRequest
{
    public IFormFile File { get; set; }

    public Guid RowId { get; set; }
    public DateTime CreatedAt { get; set; }
}