namespace Shifty.Application.Users.Queries.GetNameById;

public record GetNameByIdQuery(
    Guid Id
) : IRequest<string>;