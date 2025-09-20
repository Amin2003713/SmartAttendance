namespace App.Handlers.Universities.Queries.CheckDomain;

public record CheckDomainResponse(
    bool   Exist,
    string Message
);