using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Base.MinIo.Requests.Commands.UploadPdf;

public class UploadPdfRequest
{
    public byte[] File { get; set; } = [];
    public string FileName { get; set; }

    public Guid RowId { get; set; } = Guid.CreateVersion7();

    public FileStorageType RowType { get; set; } = FileStorageType.PdfExports;
}