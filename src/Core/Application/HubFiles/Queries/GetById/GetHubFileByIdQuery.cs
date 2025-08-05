using Shifty.Domain.HubFiles;

namespace Shifty.Application.HubFiles.Queries.GetById;

public record GetHubFileByIdQuery(
    Guid Id
) : IRequest<HubFile>;