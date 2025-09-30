using SmartAttendance.Application.Interfaces.Subjects;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Subjects;

public class SubjectPlansQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<SubjectPlans>>          logger,
    IStringLocalizer<SubjectPlans> localizer
)
    : QueryRepository<SubjectPlans>(dbContext, logger),
        ISubjectPlansQueryRepository;