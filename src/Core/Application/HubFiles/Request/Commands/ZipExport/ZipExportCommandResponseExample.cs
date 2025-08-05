using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.HubFiles.Request.Commands.ZipExport;

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