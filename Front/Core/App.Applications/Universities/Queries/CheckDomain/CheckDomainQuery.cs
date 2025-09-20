using MediatR;

namespace App.Applications.Universities.Queries.CheckDomain;

public record CheckDomainQuery(
    string Domain
) : IRequest<CheckDomainResponse>;