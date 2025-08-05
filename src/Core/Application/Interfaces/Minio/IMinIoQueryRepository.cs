using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Common.Utilities.InjectionHelpers;

namespace Shifty.Application.Interfaces.Minio;

public interface IMinIoQueryRepository : IScopedDependency
{
    Task<Stream> GetFileAsync(string filePath, CancellationToken cancellationToken = default);
}