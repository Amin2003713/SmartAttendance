using SmartAttendance.Application.Base.MinIo.Requests.Commands.UploadPdf;

namespace SmartAttendance.Application.Base.MinIo.Commands.UploadPdf;

public class UploadPdfCommand : UploadPdfRequest,
                                IRequest<string>
{
    public UploadPdfCommand(string path, UploadPdfRequest file, Guid userId)
    {
        Path       = path;
        FileUpload = file;
        UserId     = userId;
    }

    public UploadPdfCommand() { }

    public string Path { get; set; } = null!;
    public UploadPdfRequest FileUpload { get; set; } = null!;
    public Guid UserId { get; set; }
}