using Shifty.Application.Companies.Responses.GetCompanyInfo;

namespace Shifty.Application.Companies.Queries.GetCompanyInfo;

public record GetCompanyInfoQuery(
    string Domain
) : IRequest<GetCompanyInfoResponse>;