using SmartAttendance.Common.Common.Responses.Users.Queries.Base;

namespace SmartAttendance.Application.Features.Users.Queries.GetByIds;


public class GetTeacherByIds(
    List<Guid> Ids
) : IRequest<List<GetUserResponse>>;