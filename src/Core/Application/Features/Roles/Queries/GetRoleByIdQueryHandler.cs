using SmartAttendance.Application.Features.Roles.Responses;

namespace SmartAttendance.Application.Features.Roles.Queries;

public sealed class GetRoleByIdQueryHandler(
    IRoleReadService read
) : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await read.GetByIdAsync(new RoleId(request.Id), cancellationToken) ?? throw new KeyNotFoundException("نقش یافت نشد.");
        return new RoleDto
        {
            Id = role.Id.Value,
            Name = role.Name
        };
    }
}