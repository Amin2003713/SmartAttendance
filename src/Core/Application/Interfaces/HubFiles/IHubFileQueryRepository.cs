using SmartAttendance.Application.Base.HubFiles.Commands.UploadHubFile;
using SmartAttendance.Application.Base.HubFiles.Commands.ZipExport;
using SmartAttendance.Application.Base.HubFiles.Request.Queries.GetFile;

namespace SmartAttendance.Application.Interfaces.HubFiles;

public interface IHubFileQueryRepository : IQueryRepository<HubFile>,
    IScopedDependency
{
    Task<FileTransferResponse> GetHubFile(
        Guid              rowId,
        FileType          fileType,
        FileStorageType   referenceType,
        CancellationToken cancellationToken);

    Task<string> GetBucketPath(UploadHubFileCommand request, Guid? userId, CancellationToken cancellationToken);

    Task<List<HubFile?>> GetZipAsync(ZipExportCommand ZipFile, CancellationToken cancellationToken);
}