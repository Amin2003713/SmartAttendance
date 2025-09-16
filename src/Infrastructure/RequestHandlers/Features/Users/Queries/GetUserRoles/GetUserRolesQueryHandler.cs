using SmartAttendance.Application.Features.Users.Requests.Queries.GetUserRoles;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.Utilities.EnumHelpers;

namespace SmartAttendance.RequestHandlers.Features.Users.Queries.GetUserRoles;

public class
    GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery,
    IDictionary<string, List<KeyValuePair<RolesType, string>>>>
{
    public Task<IDictionary<string, List<KeyValuePair<RolesType, string>>>> Handle(
        GetUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IDictionary<string, List<KeyValuePair<RolesType, string>>>>(Enum.GetValues<RolesType>()
            .Where(r => r.ToString().StartsWith("Admin_"))
            .Select(groupRole =>
            {
                var category = groupRole.ToString().Split('_', 2)[1];

                var children = Enum.GetValues<RolesType>().Where(r => r.GetEnglishName().StartsWith($"{category}_")).ToList();

                return new
                {
                    GroupKey = groupRole.GetEnglishName(),
                    Items = new[]
                        {
                            groupRole
                        }.Concat(children)
                        .Select(r => new KeyValuePair<RolesType, string>(
                            r,
                            r.GetDisplayName()))
                        .ToList()
                };
            })
            .Concat(new[]
            {
                new
                {
                    GroupKey = RolesType.Admin.GetEnglishName(),
                    Items
                        = new List<KeyValuePair<RolesType, string>>
                        {
                            new (
                                RolesType.Admin,
                                RolesType.Admin.GetDisplayName())
                        }
                }
            })
            .ToDictionary(a => a.GroupKey, a => a.Items));
    }
}