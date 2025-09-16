using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Application.Base.HubFiles.Request.Commands.ZipExport;

public class ZipExportCommandRequestExample : IExamplesProvider<ZipExportCommandRequest>
{
    public ZipExportCommandRequest GetExamples()
    {
        return new ZipExportCommandRequest
        {
            FromDate = DateTime.UtcNow.AddDays(-7),
            ToDate   = DateTime.UtcNow,
            RowType  = FileStorageType.Picture
        };
    }
}