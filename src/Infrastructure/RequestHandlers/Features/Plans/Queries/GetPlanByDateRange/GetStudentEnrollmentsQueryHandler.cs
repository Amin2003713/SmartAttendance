using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Interfaces.Plans;

namespace SmartAttendance.RequestHandlers.Features.Plans.Queries.GetPlanByDateRange;

public class GetStudentEnrollmentsQueryHandler(
    IPlanEnrollmentQueryRepository enrollmentQueryRepo
) : IRequestHandler<GetStudentEnrollmentsQuery, List<GetEnrollmentResponse>>
{
    public async Task<List<GetEnrollmentResponse>> Handle(GetStudentEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var enrollments = await enrollmentQueryRepo.TableNoTracking
            .Include(e => e.Plan)
            .Where(e => e.StudentId == request.StudentId)
            .ToListAsync(cancellationToken);

        return enrollments.Adapt<List<GetEnrollmentResponse>>();
    }
}