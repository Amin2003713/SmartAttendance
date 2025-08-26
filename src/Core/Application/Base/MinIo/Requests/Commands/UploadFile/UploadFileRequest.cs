using Microsoft.AspNetCore.Http;
using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Base.MinIo.Requests.Commands.UploadFile;

public class UploadFileRequest
{
    public IFormFile File { get; set; }

    public Guid RowId { get; set; }
    public DateTime ReportDate { get; set; }
    public FileStorageType RowType { get; set; }
}