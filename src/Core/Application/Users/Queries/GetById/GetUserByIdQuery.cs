using Shifty.Application.Users.Requests.Queries.GetUserInfo.GetById;

namespace Shifty.Application.Users.Queries.GetById;

public record GetUserByIdQuery(
    Guid UserId
) : IRequest<GetUserByIdResponse>;