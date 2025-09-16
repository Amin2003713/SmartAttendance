using SmartAttendance.Application.Base.HubFiles.Commands.ZipExport;
using SmartAttendance.Application.Base.MinIo.Commands.UploadPdf;
using SmartAttendance.Application.Base.MinIo.Requests.Commands.UploadFile;

namespace SmartAttendance.Application.Interfaces.HubFiles;

public interface IHubFileCommandRepository : ICommandRepository<HubFile>,
                                             IScopedDependency
{
    Task<HubFile> PostFile(UploadFileRequest   uploadFileRequest, CancellationToken cancellationToken);
    Task<string>  GetZipAsync(ZipExportCommand ZipFile,           CancellationToken cancellationToken);

    Task<string> SavePdfFile(UploadPdfCommand file, CancellationToken cancellationToken);

    // Task<SaveExcelCommandBrokerResponse>
    //     SaveExcelFile(SaveExcelCommandBroker file, CancellationToken cancellationToken);
    //
    // Task<SaveXmlCommandBrokerResponse> SaveXmlFile(SaveXmlCommandBroker file, CancellationToken cancellationToken);
}