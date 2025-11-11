using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Attendances.Queries.GetByPlan;
using SmartAttendance.Application.Interfaces.Attendances;
using SmartAttendance.Application.Features.Attendances.Responses;

namespace SmartAttendance.Application.Features.Attendances.Queries;

public class GetAttendancesByPlanQueryHandler(
    IAttendanceQueryRepository queryRepository
) : IRequestHandler<GetAttendancesByPlanQuery, List<GetAttendanceInfoResponse>>
{
    public async Task<List<GetAttendanceInfoResponse>> Handle(GetAttendancesByPlanQuery request, CancellationToken cancellationToken)
    {
        var attendances = await queryRepository.TableNoTracking
            .Include(a => a.Enrollment)
            .ThenInclude(e => e.Student)
            .Include(a => a.Excuse)
            .Where(a => a.Enrollment.PlanId == request.PlanId)
            .ToListAsync(cancellationToken);

        return attendances.Adapt<List<GetAttendanceInfoResponse>>();
    }
}