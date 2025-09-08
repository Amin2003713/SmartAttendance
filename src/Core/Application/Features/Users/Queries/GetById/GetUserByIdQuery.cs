using SmartAttendance.Application.Features.Users.Requests.Queries.GetUserInfo.GetById;

namespace SmartAttendance.Application.Features.Users.Queries.GetById;

public record GetUserByIdQuery(
    Guid UserId
) : IRequest<GetUserByIdResponse>;