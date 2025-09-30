using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Persistence.Repositories.Features.Plans;

public class PlanQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<Plan>>          logger,
    IStringLocalizer<Plan> localizer
)
    : QueryRepository<Plan>(dbContext, logger),
        IPlanQueryRepository;