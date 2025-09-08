using System.Text;

namespace SmartAttendance.Application.Base.HubFiles.Request.Queries.GetFile;

public class FileTransferResponseExample : IExamplesProvider<FileTransferResponse>
{
    public FileTransferResponse GetExamples()
    {
        var bytes = Encoding.UTF8.GetBytes("Sample content");
        return new FileTransferResponse(bytes, "file.txt");
    }
}