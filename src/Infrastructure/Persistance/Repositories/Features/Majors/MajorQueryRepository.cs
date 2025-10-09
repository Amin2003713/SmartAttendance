using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class MajorQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<Major>>          logger,
    IStringLocalizer<Major> localizer
)
    : QueryRepository<Major>(dbContext, logger),
        IMajorQueryRepository;