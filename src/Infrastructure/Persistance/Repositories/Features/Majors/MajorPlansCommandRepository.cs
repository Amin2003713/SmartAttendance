using SmartAttendance.Application.Interfaces.Subjects;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Subjects;

public class SubjectPlansCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<SubjectPlans>> logger
)
    : CommandRepository<SubjectPlans>(dbContext, logger),
        ISubjectPlansCommandRepository { }