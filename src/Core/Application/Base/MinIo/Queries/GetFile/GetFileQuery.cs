using System.IO;

namespace Shifty.Application.Base.MinIo.Queries.GetFile;

public class GetFileQuery : IRequest<Stream>
{
    public string FilePath { get; set; }
}