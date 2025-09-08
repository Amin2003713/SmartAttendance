namespace SmartAttendance.Application.Base.Companies.Queries.CheckDomain;

public record CheckDomainQuery(
    string Domain
) : IRequest<CheckDomainResponse>;