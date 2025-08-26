using Shifty.Application.Features.Users.Requests.Queries.GetUserInfo.GetById;

namespace Shifty.Application.Features.Users.Queries.GetById;

public record GetUserByIdQuery(
    Guid UserId
) : IRequest<GetUserByIdResponse>;