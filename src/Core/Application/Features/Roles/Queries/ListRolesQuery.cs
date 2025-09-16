using SmartAttendance.Application.Features.Roles.Responses;

namespace SmartAttendance.Application.Features.Roles.Queries;

public sealed record ListRolesQuery : IRequest<IReadOnlyList<RoleDto>>;