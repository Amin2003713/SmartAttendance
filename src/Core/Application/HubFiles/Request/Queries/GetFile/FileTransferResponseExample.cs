using System.Text;

namespace Shifty.Application.HubFiles.Request.Queries.GetFile;

public class FileTransferResponseExample : IExamplesProvider<FileTransferResponse>
{
    public FileTransferResponse GetExamples()
    {
        var bytes = Encoding.UTF8.GetBytes("Sample content");
        return new FileTransferResponse(bytes, "file.txt");
    }
}