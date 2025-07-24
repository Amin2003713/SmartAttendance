using Shifty.Common.General.Enums;

namespace Shifty.Application.Users.Requests.Queries.GetUserRoles;

public record GetUserRolesQuery : IRequest<IDictionary<string, List<KeyValuePair<Roles, string>>>>;