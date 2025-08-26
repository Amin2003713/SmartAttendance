using Shifty.Common.General.Enums;

namespace Shifty.Application.Features.Users.Requests.Queries.GetUserRoles;

public record GetUserRolesQuery : IRequest<IDictionary<string, List<KeyValuePair<Roles, string>>>>;