using SmartAttendance.Application.Base.Companies.Responses.GetCompanyInfo;

namespace SmartAttendance.Application.Base.Companies.Queries.GetCompanyInfo;

public record GetCompanyInfoQuery(
    string Domain
) : IRequest<GetCompanyInfoResponse>;