using SmartAttendance.Application.Features.Subjects.Responses;

namespace SmartAttendance.Application.Features.Subjects.Queries.GetAll;

public record GetAllSubjectsQuery() : IRequest<List<GetSubjectInfoResponse>> ;