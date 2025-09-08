using SmartAttendance.Application.Base.HubFiles.Queries.GetById;
using SmartAttendance.Application.Interfaces.HubFiles;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.HubFiles;

namespace SmartAttendance.RequestHandlers.Base.HubFiles.Queries.GetById;

public class GetHubFileByIdQueryHandler(
    IHubFileQueryRepository hubFileQueryRepository,
    IStringLocalizer<GetHubFileByIdQueryHandler> localizer
)
    : IRequestHandler<GetHubFileByIdQuery, HubFile>
{
    public async Task<HubFile> Handle(GetHubFileByIdQuery request, CancellationToken cancellationToken)
    {
        if (!await hubFileQueryRepository.AnyAsync(file => file.Id == request.Id, cancellationToken))
            throw SmartAttendanceException.NotFound(localizer["file not found"].Value);

        return await hubFileQueryRepository.FirstOrDefaultsAsync(a => a.Id == request.Id, cancellationToken);
    }
}