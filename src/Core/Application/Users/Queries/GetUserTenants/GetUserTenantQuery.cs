namespace Shifty.Application.Users.Queries.GetUserTenants;

public record GetUserTenantQuery(
    string UserName
) : IRequest<List<GetUserTenantResponse>>;