using Shifty.Domain.HubFiles;

namespace Shifty.Application.Base.HubFiles.Queries.GetById;

public record GetHubFileByIdQuery(
    Guid Id
) : IRequest<HubFile>;