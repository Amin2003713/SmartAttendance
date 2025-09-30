using SmartAttendance.Common.Common.Responses.Users.Queries.Base;

namespace SmartAttendance.Application.Features.Teachers.Queries.GetByIds;


public class GetTeacherByIds(
    List<Guid> Ids
) : IRequest<List<GetUserResponse>>;