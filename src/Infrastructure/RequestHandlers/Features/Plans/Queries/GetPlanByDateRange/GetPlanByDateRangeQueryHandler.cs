using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Plans.Queries.GetByDate;
using SmartAttendance.Application.Features.Plans.Responses;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.General.Enums.Attendance;
using SmartAttendance.Common.General.Enums.Excuse;
using SmartAttendance.Common.General.Enums.Plans.Enrollment;
using SmartAttendance.Domain.Features.Plans;
using SmartAttendance.Domain.Users;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Plans.Queries.GetPlanByDateRange;

public class GetPlanByDateRangeQueryHandler(
    IPlanQueryRepository planQueryRepository,
    IdentityService identityService
) : IRequestHandler<GetPlanByDateRangeQuery, List<GetPlanInfoResponse>>
{
    public async Task<List<GetPlanInfoResponse>> Handle(GetPlanByDateRangeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = identityService.GetUserId();
            var role   = identityService.GetRoles();

            var fromDate = request.From.Date;
            var toDate   = request.To.Date;

            var query = planQueryRepository.TableNoTracking.Include(a => a.Enrollments).ThenInclude(a=>a.Student)
                .Include(a => a.Enrollments)
                .ThenInclude(a => a.Attendance)
                .ThenInclude(a => a.Excuse)
                .Include(a => a.Subjects)
                .ThenInclude(a => a.Subject)
                .Include(a => a.Major)
                .Include(a => a.Teacher)
                .ThenInclude(a => a.Teacher)
                .Where(p => p.IsActive &&
                            p.StartTime.Date >= fromDate &&
                            p.StartTime.Date <= toDate);

            query = role switch
                    {
                        Roles.Admin      => query,
                        Roles.Student    => query,
                        Roles.Teacher    => query.Where(p => p.Teacher.Any(t => t.TeacherId == userId)),
                        Roles.HeadMaster => query.Where(p => p.Major == null || p.Major.HeadMasterId == userId),
                        _                => Enumerable.Empty<Plan>().AsQueryable()
                    };


            var qRes = (await query.ToListAsync(cancellationToken));

            if (qRes.Count == 0)
                return [];

            return qRes.Adapt<List<GetPlanInfoResponse>>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}


public class GetDashboardDataQueryHandler(
    IPlanQueryRepository planQueryRepository,
    IdentityService identityService,
    UserManager<User> userManager, IMajorQueryRepository majorQueryRepository , ISubjectQueryRepository subjectQueryRepository
    
) : IRequestHandler<GetDashboardDataQuery, GetDashboardDataResponse>
{
    public async Task<GetDashboardDataResponse> Handle(GetDashboardDataQuery request, CancellationToken ct)
    {
        var userId = identityService.GetUserId();
        var roles  = identityService.GetRoles();

        var fromDate = request.From?.Date ?? DateTime.Today.AddDays(-30);
        var toDate   = request.To?.Date ?? DateTime.Today.AddDays(30);

        // Base query for plans with all needed includes
        var baseQuery = planQueryRepository.TableNoTracking
            .Include(p => p.Major)
            .Include(p => p.Subjects!).ThenInclude(ps => ps.Subject)
            .Include(p => p.Teacher!).ThenInclude(pt => pt.Teacher)
            .Include(p => p.Enrollments!)
                .ThenInclude(e => e.Student)
            .Include(p => p.Enrollments!)
                .ThenInclude(e => e.Attendance!)
                    .ThenInclude(a => a.Excuse)
            .Where(p => p.IsActive &&
                        p.StartTime.Date >= fromDate &&
                        p.StartTime.Date <= toDate);

        // Role-based filtering
        IQueryable<Plan> filteredQuery = roles switch
        {
            Roles.Admin      => baseQuery,
            Roles.Student    => baseQuery.Where(p => p.Enrollments!.Any(e => e.StudentId == userId)),
            Roles.Teacher    => baseQuery.Where(p => p.Teacher.Any(t => t.TeacherId == userId)),
            Roles.HeadMaster => baseQuery.Where(p => p.Major == null || p.Major.HeadMasterId == userId),
            _                => Enumerable.Empty<Plan>().AsQueryable()
        };

        var plans = await filteredQuery
            .OrderBy(p => p.StartTime)
            .ToListAsync(ct);

        var planDtos = plans.Adapt<List<GetPlanInfoResponse>>();


        var totalEnrollments = plans.Sum(p => p.Enrollments?.Count ?? 0);

        var waitlistedCount = plans
            .SelectMany(p => p.Enrollments!)
            .Count(e => e.Status == EnrollmentStatus.Waitlisted);

        var todayAbsent = plans
            .Where(p => p.StartTime.Date == DateTime.Today)
            .SelectMany(p => p.Enrollments!)
            .Count(e => 
                e.Attendance == null || 
                (e.Attendance.Status == AttendanceStatus.Absent && 
                 (e.Attendance.Excuse == null || e.Attendance.Excuse.Status != ExcuseStatus.Approved))
            );

        // === Global Counts (efficient, single queries) ===
        var students    = await userManager.GetUsersInRoleAsync("Student");
        var teachers    = await userManager.GetUsersInRoleAsync("Teacher");
        var headMasters = await userManager.GetUsersInRoleAsync("HeadMaster"); // Confirm exact role name!

        var totalStudents    = students.Count;
        var totalTeachers    = teachers.Count;
        var totalHeadMasters = headMasters.Count;

        var totalMajors   = await majorQueryRepository.TableNoTracking.CountAsync(m => m.IsActive, ct);
        var totalSubjects = await subjectQueryRepository.TableNoTracking.CountAsync(s => s.IsActive, ct);
    

        return new GetDashboardDataResponse
        {
            Plans               = planDtos,
            TotalPlans          = plans.Count,
            TotalEnrollments    = totalEnrollments,
            WaiteListed          = waitlistedCount,
            TodayAbsent         = todayAbsent,
            TotalStudents       = totalStudents,
            TotalTeachers       = totalTeachers,
            TotalHeadMaster    = totalHeadMasters,
            TotalMajor         = totalMajors,
            TotalSubjects       = totalSubjects,
            GeneratedAt         = DateTime.UtcNow
        };
    }
}