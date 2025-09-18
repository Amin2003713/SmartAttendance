using SmartAttendance.Application.Features.Users.Requests.Queries.GetUserRoles;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.Utilities.EnumHelpers;

namespace SmartAttendance.RequestHandlers.Features.Users.Queries.GetUserRoles;

public class
    GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery,
    IDictionary<string, List<KeyValuePair<Roles, string>>>>
{
    public Task<IDictionary<string, List<KeyValuePair<Roles, string>>>> Handle(
        GetUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IDictionary<string, List<KeyValuePair<Roles, string>>>>(Enum.GetValues<Roles>()
            .Where(r => r.ToString().StartsWith("Admin_"))
            .Select(groupRole =>
            {
                var category = groupRole.ToString().Split('_', 2)[1];

                var children = Enum.GetValues<Roles>().Where(r => r.GetEnglishName().StartsWith($"{category}_")).ToList();

                return new
                {
                    GroupKey = groupRole.GetEnglishName(),
                    Items = new[]
                        {
                            groupRole
                        }.Concat(children)
                        .Select(r => new KeyValuePair<Roles, string>(
                            r,
                            r.GetDisplayName()))
                        .ToList()
                };
            })
            .Concat(new[]
            {
                new
                {
                    GroupKey = Roles.Admin.GetEnglishName(),
                    Items
                        = new List<KeyValuePair<Roles, string>>
                        {
                            new (
                                Roles.Admin,
                                Roles.Admin.GetDisplayName())
                        }
                }
            })
            .ToDictionary(a => a.GroupKey, a => a.Items));
    }
}