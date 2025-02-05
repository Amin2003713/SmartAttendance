using MediatR;
using Shifty.Application.Companies.Responces.GetCompanyInfo;

namespace Shifty.Application.Companies.Queries.GetCompanyInfo
{
    public record GetCompanyInfoQuery(string Domain) : IRequest<GetCompanyInfoResponse>;
}