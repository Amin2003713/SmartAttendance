using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class SubjectPlansQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<SubjectPlans>>          logger,
    IStringLocalizer<SubjectPlans> localizer
)
    : QueryRepository<SubjectPlans>(dbContext, logger),
        ISubjectPlansQueryRepository;