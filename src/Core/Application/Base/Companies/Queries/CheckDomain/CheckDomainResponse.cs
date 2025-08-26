namespace Shifty.Application.Base.Companies.Queries.CheckDomain;

public record CheckDomainResponse(
    bool Exist,
    string Message
);