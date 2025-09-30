using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class MajorPlansCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<MajorPlans>> logger
)
    : CommandRepository<MajorPlans>(dbContext, logger),
        IMajorPlansCommandRepository { }