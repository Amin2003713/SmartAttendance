namespace Shifty.Application.Features.Users.Queries.GetNameById;

public record GetNameByIdQuery(
    Guid Id
) : IRequest<string>;