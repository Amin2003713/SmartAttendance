using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Base.HubFiles.Commands.ZipExport;
using Shifty.Application.Base.MinIo.Commands.UploadPdf;
using Shifty.Application.Base.MinIo.Requests.Commands.UploadFile;
using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.HubFiles;

namespace Shifty.Application.Interfaces.HubFiles;

public interface IHubFileCommandRepository : ICommandRepository<HubFile>,
    IScopedDependency
{
    Task<HubFile> PostFile(UploadFileRequest uploadFileRequest, CancellationToken cancellationToken);
    Task<string> GetZipAsync(ZipExportCommand ZipFile, CancellationToken cancellationToken);

    Task<string> SavePdfFile(UploadPdfCommand file, CancellationToken cancellationToken);

    // Task<SaveExcelCommandBrokerResponse>
    //     SaveExcelFile(SaveExcelCommandBroker file, CancellationToken cancellationToken);
    //
    // Task<SaveXmlCommandBrokerResponse> SaveXmlFile(SaveXmlCommandBroker file, CancellationToken cancellationToken);
}