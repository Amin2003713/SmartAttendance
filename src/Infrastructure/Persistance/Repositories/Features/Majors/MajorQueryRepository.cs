using SmartAttendance.Application.Interfaces.Subjects;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Subjects;

public class SubjectQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<Subject>>          logger,
    IStringLocalizer<Subject> localizer
)
    : QueryRepository<Subject>(dbContext, logger),
        ISubjectQueryRepository;