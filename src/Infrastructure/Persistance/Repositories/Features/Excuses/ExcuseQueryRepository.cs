using SmartAttendance.Application.Interfaces.Excuses;
using SmartAttendance.Domain.Features.Excuses;

namespace SmartAttendance.Persistence.Repositories.Features.Excuses;

public class ExcuseQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<Excuse>>          logger,
    IStringLocalizer<Excuse> localizer
)
    : QueryRepository<Excuse>(dbContext, logger),
        IExcuseQueryRepository;