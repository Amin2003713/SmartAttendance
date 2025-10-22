using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Application.Base.MinIo.Requests.Commands.UploadPdf;

public class UploadPdfRequest
{
    public byte[] File { get; set; } = [];
    public string FileName { get; set; }

    public Guid RowId { get; set; } = Guid.CreateVersion7();

}