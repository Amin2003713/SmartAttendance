using System.IO;
using Microsoft.AspNetCore.Http;
using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Base.HubFiles.Request.Commands.UploadHubFile;

public class UploadHubFileRequest
{
    public IFormFile File { get; set; }


    public DateTime ReportDate { get; set; }
    public FileStorageType RowType { get; set; }

    public Guid RowId { get; set; }


    public string FileExtension()
    {
        return (Path.GetExtension(File?.FileName) ?? string.Empty).Replace(".", "");
    }
}