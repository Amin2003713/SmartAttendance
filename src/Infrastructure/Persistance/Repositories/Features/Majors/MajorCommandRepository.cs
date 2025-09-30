using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class SubjectCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<Subject>> logger
)
    : CommandRepository<Subject>(dbContext, logger),
        ISubjectCommandRepository { }