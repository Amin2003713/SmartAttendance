using SmartAttendance.Application.Features.Subjects.Responses;

namespace SmartAttendance.Application.Features.Subjects.Queries.GetSubjectsByMajor;

public record GetSubjectsByMajorQuery(
    Guid MajorId
) : IRequest<List<GetSubjectInfoResponse>>;