using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Commons.Queries.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Application.Features.Users.Queries.GetAllUsers;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.RequestHandlers.Features.Users.Queries.GetAllUsers;

public class GetAllUserQueryHandler(
    IQueryRepository<User>          queryRepository,
    IMediator                       mediator,
    UserManager<User>               userManager,
    ILogger<GetAllUserQueryHandler> logger
) : IRequestHandler<GetAllUsersQuery, List<GetUserResponse>>
{
    public async Task<List<GetUserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users   = await queryRepository.TableNoTracking.ToListAsync(cancellationToken);
        var results = new List<GetUserResponse>();

        logger.LogInformation("Retrieved {Count} users from the database.", users.Count);

        foreach (var user in users)
        {
            var result = user.Adapt<GetUserResponse>();

            if (user.CreatedBy != null)
                result.CreatedBy =
                    await mediator.Send(new GetLogPropertyInfoQuery(user.CreatedBy.Value), cancellationToken);

            result.Roles = (await userManager.GetRolesAsync(user) as List<string>)!.Select(NormalizeName).ToList();

            results.Add(result);
        }

        logger.LogInformation("Mapped {Count} users to response with role information.", results.Count);

        return results;
    }

    private string NormalizeName(string name)
    {
        if (IsPascalCase(name))
            return ToCamelCase(name);

        if (IsSnakeCase(name))
            return name.ToLowerInvariant();

        return name.ToLower();
    }

    private bool IsPascalCase(string s)
    {
        return !string.IsNullOrWhiteSpace(s)         &&
               char.IsUpper(s[0])                    &&
               (s.Length == 1 || char.IsLower(s[1])) &&
               !s.Contains('_');
    }

    private bool IsSnakeCase(string s)
    {
        return s.Contains('_') && char.IsUpper(s[0]);
        // snake_case but not yet lowercase
    }

    private string ToCamelCase(string s)
    {
        return char.ToLowerInvariant(s[0]) + s.Substring(1);
    }
}