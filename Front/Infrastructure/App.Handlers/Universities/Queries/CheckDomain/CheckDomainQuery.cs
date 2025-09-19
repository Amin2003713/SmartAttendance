using MediatR;

namespace SmartAttendance.Application.Base.Universities.Queries.CheckDomain;

public record CheckDomainQuery(
    string Domain
) : IRequest<CheckDomainResponse>;