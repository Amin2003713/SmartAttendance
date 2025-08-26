using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Base.HubFiles.Request.Commands.UploadHubFile;

public class UploadHubFileRequestExample : IExamplesProvider<UploadHubFileRequest>
{
    public UploadHubFileRequest GetExamples()
    {
        return new UploadHubFileRequest
        {
            File = null!,

            ReportDate = DateTime.UtcNow,
            RowId = Guid.NewGuid(),
            RowType = FileStorageType.Material
        };
    }
}