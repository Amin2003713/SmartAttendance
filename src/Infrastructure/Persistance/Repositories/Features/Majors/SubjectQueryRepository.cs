using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class SubjectQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<Subject>>          logger,
    IStringLocalizer<Subject> localizer
)
    : QueryRepository<Subject>(dbContext, logger),
        ISubjectQueryRepository;