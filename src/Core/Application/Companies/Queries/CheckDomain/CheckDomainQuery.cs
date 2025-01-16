using MediatR;
using Shifty.Application.Companies.Queries.GetCompanyInfo;

namespace Shifty.Persistence.CommandHandlers.Companies.Queries.CheckDomain
{
    public record CheckDomainQuery(string Domain)   :  IRequest<CheckDomainResponse>;

    public record CheckDomainResponse(bool Available , string Message );
}