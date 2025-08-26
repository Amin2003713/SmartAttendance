namespace Shifty.Application.Features.Users.Queries.GetUserTenants;

public record GetUserTenantQuery(
    string UserName
) : IRequest<List<GetUserTenantResponse>>;