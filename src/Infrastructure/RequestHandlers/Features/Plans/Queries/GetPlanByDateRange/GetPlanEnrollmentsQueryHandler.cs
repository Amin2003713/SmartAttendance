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
            .Where(e => e.PlanId == request.PlanId)
            .ToListAsync(cancellationToken);

        return enrollments.Adapt<List<GetEnrollmentResponse>>();
    }
}