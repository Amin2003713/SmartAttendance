using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Plans.Queries.ListAllByStudentMajor;
using SmartAttendance.Application.Features.Plans.Responses;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Plans.Queries.GetPlanByDateRange;

public class ListAllByStudentQueryHandler(
    IPlanQueryRepository queryRepository,
    IPlanEnrollmentQueryRepository enrollmentQueryRepository
) : IRequestHandler<ListAllByStudentQuery, List<GetPlanInfoResponse>>
{
    public async Task<List<GetPlanInfoResponse>> Handle(ListAllByStudentQuery request, CancellationToken cancellationToken)
    {
        // Get plan IDs where the student is enrolled
        var enrolledPlanIds = await enrollmentQueryRepository.TableNoTracking
            .Where(e => e.StudentId == request.StudentId)
            .Select(e => e.PlanId)
            .ToListAsync(cancellationToken);

        if (enrolledPlanIds.Count == 0)
            throw SmartAttendanceException.NotFound("No enrolled plans found for this student");

        // Load the corresponding plans
        var plans = await queryRepository.TableNoTracking
            .Include(a => a.Subjects)
            .Include(a => a.Teacher)
            .Include(a => a.Major)
            .Where(p => enrolledPlanIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        return plans.Adapt<List<GetPlanInfoResponse>>();
    }
}