using MediatR;

namespace Shifty.Application.Companies.Queries.CheckDomain
{
    public record CheckDomainQuery(string Domain) : IRequest<CheckDomainResponse>;
}