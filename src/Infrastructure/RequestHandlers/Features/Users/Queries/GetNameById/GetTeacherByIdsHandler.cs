using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Users.Queries.GetByIds;
using SmartAttendance.Application.Interfaces.Users;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;

namespace SmartAttendance.RequestHandlers.Features.Users.Queries.GetNameById;

public class GetTeacherByIdsHandler(
    IUserQueryRepository queryRepository,
    IStringLocalizer<GetTeacherByIds> localizer,
    ILogger<GetTeacherByIdsHandler>   logger
) : IRequestHandler<GetTeacherByIds, List<GetUserResponse>>
{
    public async Task<List<GetUserResponse>> Handle(GetTeacherByIds request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var result = await queryRepository.TableNoTracking.Where(a => request.Ids.Contains(a.Id)).ToListAsync(cancellationToken);
            return result.Select(a => a.Adapt<GetUserResponse>()).ToList();
        }
        catch (Exception e)
        {
            throw;
        }
    }
}