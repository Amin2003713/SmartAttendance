namespace SmartAttendance.Application.Base.Companies.Commands.AddRequest;

public record AddRequestCommand(
    string EndPoint,
    string TenantId,
    Guid? UserId,
    string CorrelationId,
    string ServiceName
) : IRequest;