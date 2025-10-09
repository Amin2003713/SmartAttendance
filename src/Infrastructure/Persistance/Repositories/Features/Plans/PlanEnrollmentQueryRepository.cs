using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Domain.Features.PlanEnrollments;

namespace SmartAttendance.Persistence.Repositories.Features.Plans;

public class PlanEnrollmentQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<PlanEnrollment>>          logger,
    IStringLocalizer<PlanEnrollment> localizer
)
    : QueryRepository<PlanEnrollment>(dbContext, logger),
        IPlanEnrollmentQueryRepository;