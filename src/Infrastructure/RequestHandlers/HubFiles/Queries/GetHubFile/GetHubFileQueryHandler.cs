using Shifty.Application.HubFiles.Queries.GetHubFile;
using Shifty.Application.HubFiles.Request.Queries.GetFile;
using Shifty.Application.Interfaces.HubFiles;

namespace Shifty.RequestHandlers.HubFiles.Queries.GetHubFile;

public class GetHubFileQueryHandler(
    IHubFileQueryRepository hubFileQueryRepository
)
    : IRequestHandler<GetHubFileQuery, FileTransferResponse>
{
    public async Task<FileTransferResponse> Handle(GetHubFileQuery request, CancellationToken cancellationToken)
    {
        return await hubFileQueryRepository.GetHubFile(request.FileId,
            request.FileType,
            request.ReferenceType,
            cancellationToken);
    }
}