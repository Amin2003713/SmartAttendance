using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Common.Utilities.InjectionHelpers;

namespace SmartAttendance.Application.Interfaces.Minio;

public interface IMinIoQueryRepository : IScopedDependency
{
    Task<Stream> GetFileAsync(string filePath, CancellationToken cancellationToken = default);
}