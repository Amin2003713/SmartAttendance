using SmartAttendance.Application.Base.Universities.Responses.GetCompanyInfo;

namespace SmartAttendance.Application.Base.Universities.Queries.GetUniversityInfo;

public record GetUniversityInfoQuery(
    string Domain
) : IRequest<GetUniversityInfoResponse>;