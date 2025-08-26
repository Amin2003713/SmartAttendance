using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Base.HubFiles.Request.Commands.ZipExport;

public class ZipExportCommandRequest
{

    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public FileStorageType RowType { get; set; }
}