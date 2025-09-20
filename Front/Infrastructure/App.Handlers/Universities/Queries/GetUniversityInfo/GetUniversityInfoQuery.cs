using App.Applications.Universities.Responses.GetCompanyInfo;

namespace App.Handlers.Universities.Queries.GetUniversityInfo;

public record GetUniversityInfoQuery(
    string Domain
) : IRequest<GetUniversityInfoResponse>;