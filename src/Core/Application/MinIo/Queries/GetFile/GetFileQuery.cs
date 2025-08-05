using System.IO;

namespace Shifty.Application.MinIo.Queries.GetFile;

public class GetFileQuery : IRequest<Stream>
{
    public string FilePath { get; set; }
}