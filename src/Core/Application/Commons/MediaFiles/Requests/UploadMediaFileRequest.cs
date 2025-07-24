using System.IO;
using Microsoft.AspNetCore.Http;

namespace Shifty.Application.Commons.MediaFiles.Requests;

public class UploadMediaFileRequest
{
    public string? MediaUrl { get; set; }

    public IFormFile? MediaFile { get; set; }


    public string FileExtension()
    {
        return (Path.GetExtension(MediaFile?.FileName) ?? string.Empty).Replace(".", "");
    }
}