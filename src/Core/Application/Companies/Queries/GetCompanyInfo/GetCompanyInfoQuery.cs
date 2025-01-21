using MediatR;

namespace Shifty.Application.Companies.Queries.GetCompanyInfo
{
    public record GetCompanyInfoQuery(string Domain) : IRequest<GetCompanyInfoResponse>;
}