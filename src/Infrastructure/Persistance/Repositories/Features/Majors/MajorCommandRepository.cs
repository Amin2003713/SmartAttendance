using SmartAttendance.Application.Interfaces.Subjects;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Subjects;

public class SubjectCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<Subject>> logger
)
    : CommandRepository<Subject>(dbContext, logger),
        ISubjectCommandRepository { }