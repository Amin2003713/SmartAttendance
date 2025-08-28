using Shifty.Application.Base.HubFiles.Queries.GetById;
using Shifty.Application.Interfaces.HubFiles;
using Shifty.Common.Exceptions;
using Shifty.Domain.HubFiles;

namespace Shifty.RequestHandlers.Base.HubFiles.Queries.GetById;

public class GetHubFileByIdQueryHandler(
    IHubFileQueryRepository hubFileQueryRepository,
    IStringLocalizer<GetHubFileByIdQueryHandler> localizer
)
    : IRequestHandler<GetHubFileByIdQuery, HubFile>
{
    public async Task<HubFile> Handle(GetHubFileByIdQuery request, CancellationToken cancellationToken)
    {
        if (!await hubFileQueryRepository.AnyAsync(file => file.Id == request.Id, cancellationToken))
            throw ShiftyException.NotFound(localizer["file not found"].Value);

        return await hubFileQueryRepository.FirstOrDefaultsAsync(a => a.Id == request.Id, cancellationToken);
    }
}