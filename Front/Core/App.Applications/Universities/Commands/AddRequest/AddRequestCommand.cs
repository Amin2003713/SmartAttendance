using MediatR;

namespace App.Applications.Universities.Commands.AddRequest;

public record AddRequestCommand(
    string EndPoint,
    string TenantId,
    Guid?  UserId,
    string CorrelationId,
    string ServiceName
) : IRequest;