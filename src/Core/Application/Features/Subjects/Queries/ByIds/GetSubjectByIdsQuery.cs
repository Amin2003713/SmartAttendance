using SmartAttendance.Application.Features.Subjects.Responses;

namespace SmartAttendance.Application.Features.Subjects.Queries.ByIds;

public record GetSubjectByIdsQuery(
    List<Guid> Ids
) : IRequest<List<GetSubjectInfoResponse>>;