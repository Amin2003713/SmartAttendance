using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Base.HubFiles.Commands.UploadHubFile;
using Shifty.Application.Base.HubFiles.Commands.ZipExport;
using Shifty.Application.Base.HubFiles.Request.Queries.GetFile;
using Shifty.Application.Interfaces.Base;
using Shifty.Common.General.Enums.FileType;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.HubFiles;

namespace Shifty.Application.Interfaces.HubFiles;

public interface IHubFileQueryRepository : IQueryRepository<HubFile>,
    IScopedDependency
{
    Task<FileTransferResponse> GetHubFile(
        Guid rowId,
        FileType fileType,
        FileStorageType referenceType,
        CancellationToken cancellationToken);

    Task<string> GetBucketPath(UploadHubFileCommand request, Guid? userId, CancellationToken cancellationToken);

    Task<List<HubFile?>> GetZipAsync(ZipExportCommand ZipFile, CancellationToken cancellationToken);
}