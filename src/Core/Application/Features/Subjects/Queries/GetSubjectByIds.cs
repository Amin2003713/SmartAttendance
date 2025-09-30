using SmartAttendance.Application.Features.Subjects.Responses;

namespace SmartAttendance.Application.Features.Subjects.Queries;

public class GetSubjectByIds(
    List<Guid> Ids
) : IRequest<List<GetSubjectInfoResponse>>;