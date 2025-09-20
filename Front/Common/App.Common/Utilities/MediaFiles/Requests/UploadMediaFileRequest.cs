using Microsoft.AspNetCore.Components.Forms;

namespace App.Common.Utilities.MediaFiles.Requests;

public class UploadMediaFileRequest
{
    public string? MediaUrl { get; set; }

    public IBrowserFile? MediaFile { get; set; }


    public string FileExtension()
    {
        return (Path.GetExtension(MediaFile?.Name) ?? string.Empty).Replace(".", "");
    }
}