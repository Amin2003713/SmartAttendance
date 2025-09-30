using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class MajorPlansQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<MajorPlans>>          logger,
    IStringLocalizer<MajorPlans> localizer
)
    : QueryRepository<MajorPlans>(dbContext, logger),
        IMajorPlansQueryRepository;