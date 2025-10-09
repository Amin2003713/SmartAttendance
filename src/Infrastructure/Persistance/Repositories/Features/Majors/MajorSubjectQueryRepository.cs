using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class MajorSubjectQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<MajorSubject>>          logger,
    IStringLocalizer<MajorSubject> localizer
)
    : QueryRepository<MajorSubject>(dbContext, logger),
        IMajorSubjectQueryRepository;