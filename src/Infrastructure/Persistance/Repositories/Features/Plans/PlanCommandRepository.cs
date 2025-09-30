using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Persistence.Repositories.Features.Plans;



public class PlanCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<Plan>> logger
)
    : CommandRepository<Plan>(dbContext, logger),
        IPlanCommandRepository { }