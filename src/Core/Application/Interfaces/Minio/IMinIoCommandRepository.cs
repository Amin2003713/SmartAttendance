using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Base.MinIo.Commands.UploadPdf;
using Shifty.Application.Base.MinIo.Commands.UplodeFile;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.HubFiles;

namespace Shifty.Application.Interfaces.Minio;

public interface IMinIoCommandRepository : IScopedDependency
{
    Task<HubFile> UploadFileAsync(UploadFileCommand file, CancellationToken cancellationToken);

    Task<HubFile> UploadPdfAsync(UploadPdfCommand file, CancellationToken cancellationToken);

    Task<HubFile> UploadExcelAsync(UploadPdfCommand file, CancellationToken cancellationToken);

    Task<HubFile> UploadXmlAsync(UploadPdfCommand file, CancellationToken cancellationToken);

    Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken);
    void DeletePdfFile(string filePath);
}