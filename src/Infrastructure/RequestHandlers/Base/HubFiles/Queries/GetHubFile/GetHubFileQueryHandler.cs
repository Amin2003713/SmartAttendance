using SmartAttendance.Application.Base.HubFiles.Queries.GetHubFile;
using SmartAttendance.Application.Base.HubFiles.Request.Queries.GetFile;
using SmartAttendance.Application.Interfaces.HubFiles;

namespace SmartAttendance.RequestHandlers.Base.HubFiles.Queries.GetHubFile;

public class GetHubFileQueryHandler(
    IHubFileQueryRepository hubFileQueryRepository
)
    : IRequestHandler<GetHubFileQuery, FileTransferResponse>
{
    public async Task<FileTransferResponse> Handle(GetHubFileQuery request, CancellationToken cancellationToken)
    {
        return await hubFileQueryRepository.GetHubFile(request.FileId,
            request.FileType,
            cancellationToken);
    }
}