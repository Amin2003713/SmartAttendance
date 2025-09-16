namespace SmartAttendance.Application.Interfaces.Minio;

public interface IMinIoQueryRepository : IScopedDependency
{
    Task<Stream> GetFileAsync(string filePath, CancellationToken cancellationToken = default);
}