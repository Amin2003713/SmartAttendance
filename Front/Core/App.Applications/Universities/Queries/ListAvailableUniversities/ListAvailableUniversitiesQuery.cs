using MediatR;
using SmartAttendance.Application.Base.Universities.Responses.GetCompanyInfo;

namespace SmartAttendance.Application.Base.Universities.Queries.ListAvailableUniversities;

public record ListAvailableUniversitiesQuery : IRequest<List<GetUniversityInfoResponse>>;