using MediatR;

namespace Shifty.Application.Panel.Companies.Queries.CheckDomain
{
    public record CheckDomainQuery(string Domain) : IRequest<CheckDomainResponse>;
}