using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Domain.Features.PlanEnrollments;

namespace SmartAttendance.Persistence.Repositories.Features.Plans;

public class PlanEnrollmentCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<PlanEnrollment>> logger
)
    : CommandRepository<PlanEnrollment>(dbContext, logger),
        IPlanEnrollmentCommandRepository { }