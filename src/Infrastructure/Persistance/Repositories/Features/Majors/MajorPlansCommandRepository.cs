using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class SubjectPlansCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<SubjectPlans>> logger
)
    : CommandRepository<SubjectPlans>(dbContext, logger),
        ISubjectPlansCommandRepository { }