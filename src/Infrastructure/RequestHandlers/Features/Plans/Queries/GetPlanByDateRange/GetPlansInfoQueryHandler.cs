using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Plans.Queries.GetByDate;
using SmartAttendance.Application.Features.Plans.Responses;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Domain.Features.Plans;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Plans.Queries.GetPlanByDateRange;

public class GetPlanByDateRangeQueryHandler(
    IPlanQueryRepository planQueryRepository,
    IdentityService identityService
) : IRequestHandler<GetPlanByDateRangeQuery, List<GetPlanInfoResponse>>
{
    public async Task<List<GetPlanInfoResponse>> Handle(GetPlanByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId();
        var role   = identityService.GetRoles();

        // Pre-calculate dates once for cleaner query
        var fromDate = request.From.Date;
        var toDate   = request.To.Date;

        // Base query - always filtered by date & IsActive
        var query = planQueryRepository.TableNoTracking
            .Where(p => p.IsActive &&
                        p.StartTime.Date >= fromDate &&
                        p.StartTime.Date <= toDate);

        // Apply role-based filtering
        query = role switch
                {
                    Roles.Admin      => query,
                    Roles.Student    => query.Where(p => p.Enrollments.Any(e => e.StudentId == userId)),
                    Roles.Teacher    => query.Where(p => p.Teacher.Any(t => t.TeacherId == userId)),
                    Roles.HeadMaster => query.Where(p => p.Major == null || p.Major.HeadMasterId == userId),
                    _                => Enumerable.Empty<Plan>().AsQueryable()
                };

        // Project to response DTO
        return await query
            .ProjectToType<GetPlanInfoResponse>()
            .ToListAsync(cancellationToken);
    }
}