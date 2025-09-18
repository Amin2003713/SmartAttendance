namespace SmartAttendance.Application.Base.Universities.Queries.CheckDomain;

public record CheckDomainResponse(
    bool   Exist,
    string Message
);