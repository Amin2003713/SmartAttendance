using Mapster;
using SmartAttendance.Application.Features.Users.Queries.GetUserTenants;
using SmartAttendance.Application.Interfaces.Tenants.Companies;

namespace SmartAttendance.RequestHandlers.Features.Users.Queries.GetUserTenants;

public class GetUserTenantQueryHandler(
    IUniversityRepository                 repository,
    ILogger<GetUserTenantQueryHandler> logger
) : IRequestHandler<GetUserTenantQuery, List<GetUserTenantResponse>>
{
    public async Task<List<GetUserTenantResponse>> Handle(
        GetUserTenantQuery request,
        CancellationToken  cancellationToken)
    {
        var result = await repository.FindByUserNameAsync(request.UserName, cancellationToken);

        logger.LogInformation("Retrieved {Count} tenants for user {UserName}.", result.Count, request.UserName);

        return result.Adapt<List<GetUserTenantResponse>>();
    }
}