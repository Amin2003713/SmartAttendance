using SmartAttendance.Application.Features.Roles.Responses;

namespace SmartAttendance.Application.Features.Roles.Queries;

public sealed record GetRoleByIdQuery(
    Guid Id
) : IRequest<RoleDto>;