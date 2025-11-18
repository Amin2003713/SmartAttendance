using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Interfaces.Plans;

namespace SmartAttendance.RequestHandlers.Features.Plans.Queries.GetPlanByDateRange;

public class GetPlanEnrollmentsQueryHandler(
    IPlanEnrollmentQueryRepository enrollmentQueryRepo
) : IRequestHandler<GetPlanEnrollmentsQuery, List<GetEnrollmentResponse>>
{
    public async Task<List<GetEnrollmentResponse>> Handle(GetPlanEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var enrollments = await enrollmentQueryRepo.TableNoTracking
            .Include(e => e.Plan)
            .Include(a => a.Attendance)
            .ThenInclude(a => a.Excuse)
            .Include(a => a.Student)
            .Where(e => e.PlanId == request.PlanId)
            .ToListAsync(cancellationToken);

        return enrollments.Adapt<List<GetEnrollmentResponse>>();
    }
}