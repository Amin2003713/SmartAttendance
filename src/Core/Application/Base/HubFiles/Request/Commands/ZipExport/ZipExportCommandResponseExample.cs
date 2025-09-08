using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Application.Base.HubFiles.Request.Commands.ZipExport;

public class ZipExportCommandResponseExample : IExamplesProvider<ZipExportCommandResponse>
{
    public ZipExportCommandResponse GetExamples()
    {
        return new ZipExportCommandResponse
        {
            File = null!,

            RowId = Guid.NewGuid(),
            ReportDate = DateTime.UtcNow,
            RowType = FileStorageType.ZipExports
        };
    }
}