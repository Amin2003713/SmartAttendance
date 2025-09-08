using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Application.Features.Users.Requests.Queries.GetUserRoles;

public record GetUserRolesQuery : IRequest<IDictionary<string, List<KeyValuePair<Roles, string>>>>;