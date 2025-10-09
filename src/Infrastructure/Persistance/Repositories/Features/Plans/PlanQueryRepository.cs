using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Domain.Features.PlanEnrollments;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Persistence.Repositories.Features.Plans;

public class PlanQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<Plan>>          logger,
    IStringLocalizer<Plan> localizer
)
    : QueryRepository<Plan>(dbContext, logger),
        IPlanQueryRepository;


public class PlanEnrollmentQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<PlanEnrollment>>          logger,
    IStringLocalizer<PlanEnrollment> localizer
)
    : QueryRepository<PlanEnrollment>(dbContext, logger),
        IPlanEnrollmentQueryRepository;