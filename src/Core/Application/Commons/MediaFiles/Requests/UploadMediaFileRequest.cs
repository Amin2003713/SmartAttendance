using Microsoft.AspNetCore.Http;

namespace SmartAttendance.Application.Commons.MediaFiles.Requests;

public class UploadMediaFileRequest
{
    public string? MediaUrl { get; set; }

    public IFormFile? MediaFile { get; set; }
}