using SmartAttendance.Application.Features.Roles.Responses;

namespace SmartAttendance.Application.Features.Roles.Queries;

public sealed record GetRoleByIdQuery(Guid Id) : IRequest<RoleDto>;
public sealed record ListRolesQuery() : IRequest<IReadOnlyList<RoleDto>>;

public sealed class GetRoleByIdQueryHandler(IRoleReadService read) : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
	public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
	{
		var role = await read.GetByIdAsync(new RoleId(request.Id), cancellationToken) ?? throw new KeyNotFoundException("نقش یافت نشد.");
		return new RoleDto { Id = role.Id.Value, Name = role.Name };
	}
}

public sealed class ListRolesQueryHandler(IRoleReadService read) : IRequestHandler<ListRolesQuery, IReadOnlyList<RoleDto>>
{
	public async Task<IReadOnlyList<RoleDto>> Handle(ListRolesQuery request, CancellationToken cancellationToken)
	{
		var list = await read.ListAsync(cancellationToken);
		return list.Select(r => new RoleDto { Id = r.Id.Value, Name = r.Name }).ToList();
	}
}

