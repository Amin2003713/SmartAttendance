using SmartAttendance.Application.Features.Roles.Responses;

namespace SmartAttendance.Application.Features.Roles.Queries;

public sealed class ListRolesQueryHandler(
    IRoleReadService read
) : IRequestHandler<ListRolesQuery, IReadOnlyList<RoleDto>>
{
    public async Task<IReadOnlyList<RoleDto>> Handle(ListRolesQuery request, CancellationToken cancellationToken)
    {
        var list = await read.ListAsync(cancellationToken);
        return list.Select(r => new RoleDto
            {
                Id = r.Id.Value,
                Name = r.Name
            })
            .ToList();
    }
}