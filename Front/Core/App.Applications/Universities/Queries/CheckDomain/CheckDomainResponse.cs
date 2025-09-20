namespace App.Applications.Universities.Queries.CheckDomain;

public record CheckDomainResponse(
    bool   Exist,
    string Message
);