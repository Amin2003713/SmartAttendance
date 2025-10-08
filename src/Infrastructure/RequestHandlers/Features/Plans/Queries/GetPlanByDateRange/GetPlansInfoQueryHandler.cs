using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Plans.Queries.GetByDate;
using SmartAttendance.Application.Features.Plans.Responses;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Plans.Queries.GetPlansInfo;

public class GetPlanByDateRangeQueryHandler(
    IPlanQueryRepository planQueryRepository ,
    IdentityService                  identityService,
    IPlanEnrollmentQueryRepository enrollmentQueryRepository
) :  IRequestHandler<GetPlanByDateRangeQuery, List<GetPlanInfoResponse>>
{
    public async Task<List<GetPlanInfoResponse>> Handle(GetPlanByDateRangeQuery request, CancellationToken cancellationToken)
    {
        switch (identityService.GetRoles())
        {
            case Roles.Admin:
                return await planQueryRepository.TableNoTracking.Where(a => a.IsActive &&
                                                                            a.StartTime.Date >= request.From.Date &&
                                                                            a.StartTime.Date <=
                                                                            request.To.Date )
                    .ProjectToType<GetPlanInfoResponse>()
                    .ToListAsync(cancellationToken: cancellationToken);

                break;
            case Roles.Student :
                plansId = await enrollmentQueryRepository.TableNoTracking.Where(a => a.IsActive && a.StudentId == identityService.GetUserId<Guid>())
                    .Select(a => a.PlanId)
                    .ToListAsync(cancellationToken);

                break;
            case Roles.Teacher:
                plansId = await enrollmentQueryRepository.TableNoTracking.Where(a => a.IsActive && a.StudentId == identityService.GetUserId<Guid>())
                    .Select(a => a.PlanId)
                    .ToListAsync(cancellationToken);

                break;
            case Roles.HeadMaster :
                break;
        }
    }
}