using Shifty.Application.Base.Companies.Responses.GetCompanyInfo;

namespace Shifty.Application.Base.Companies.Queries.GetCompanyInfo;

public record GetCompanyInfoQuery(
    string Domain
) : IRequest<GetCompanyInfoResponse>;