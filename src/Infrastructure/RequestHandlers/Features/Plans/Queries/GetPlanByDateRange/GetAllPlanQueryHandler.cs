using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Plans.Queries.GetAll;
using SmartAttendance.Application.Features.Plans.Responses;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Plans.Queries.GetPlanByDateRange;

public class GetAllPlanQueryHandler(
    IPlanQueryRepository queryRepository
) : IRequestHandler<GetAllPlanQuery, List<GetPlanInfoResponse>>
{
    public async Task<List<GetPlanInfoResponse>> Handle(GetAllPlanQuery request, CancellationToken cancellationToken)
    {
        var plans = await queryRepository.TableNoTracking
            .Include(a => a.Subjects)
            .Include(a => a.Teacher)
            .Include(a => a.Major)
            .ToListAsync(cancellationToken);

        if (plans.Count == 0)
            throw SmartAttendanceException.NotFound("No plans found");

        return plans.Adapt<List<GetPlanInfoResponse>>();
    }
}