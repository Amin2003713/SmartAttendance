using System.IO;
using Microsoft.AspNetCore.Http;
using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Application.Base.HubFiles.Request.Commands.ZipExport;

public class ZipExportCommandResponse
{
    public IFormFile File { get; set; }

    public string FileExtension()
    {
        return (Path.GetExtension(File?.FileName) ?? string.Empty).Replace(".", "");
    }
}