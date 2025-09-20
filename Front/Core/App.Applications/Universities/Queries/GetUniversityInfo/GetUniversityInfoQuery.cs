using App.Applications.Universities.Responses.GetCompanyInfo;
using MediatR;

namespace App.Applications.Universities.Queries.GetUniversityInfo;

public record GetUniversityInfoQuery(
    string Domain
) : IRequest<GetUniversityInfoResponse>;