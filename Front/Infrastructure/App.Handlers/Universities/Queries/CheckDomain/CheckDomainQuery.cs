namespace App.Handlers.Universities.Queries.CheckDomain;

public record CheckDomainQuery(
    string Domain
) : IRequest<CheckDomainResponse>;