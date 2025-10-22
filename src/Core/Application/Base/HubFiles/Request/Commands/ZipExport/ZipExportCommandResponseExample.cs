using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Application.Base.HubFiles.Request.Commands.ZipExport;

public class ZipExportCommandResponseExample : IExamplesProvider<ZipExportCommandResponse>
{
    public ZipExportCommandResponse GetExamples()
    {
        return new ZipExportCommandResponse
        {
            File = null!,
        };
    }
}