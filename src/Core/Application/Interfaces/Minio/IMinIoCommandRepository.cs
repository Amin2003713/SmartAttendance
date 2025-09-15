using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Base.MinIo.Commands.UploadPdf;
using SmartAttendance.Application.Base.MinIo.Commands.UplodeFile;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.HubFiles;

namespace SmartAttendance.Application.Interfaces.Minio;

public interface IMinIoCommandRepository : IScopedDependency
{
    Task<HubFile> UploadFileAsync(UploadFileCommand file, CancellationToken cancellationToken);

    Task<HubFile> UploadPdfAsync(UploadPdfCommand file, CancellationToken cancellationToken);

    Task<HubFile> UploadExcelAsync(UploadPdfCommand file, CancellationToken cancellationToken);

    Task<HubFile> UploadXmlAsync(UploadPdfCommand file, CancellationToken cancellationToken);

    Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken);
    void       DeletePdfFile(string   filePath);
}