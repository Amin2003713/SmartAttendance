using SmartAttendance.Application.Features.Subjects.Responses;

namespace SmartAttendance.Application.Features.Subjects.Queries.GetTeacherSubjects;

public record GetTeacherSubjectsQuery(
    Guid TeacherId
) : IRequest<List<GetSubjectInfoResponse>>;