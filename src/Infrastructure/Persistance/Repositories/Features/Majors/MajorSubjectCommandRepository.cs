using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class MajorSubjectCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<MajorSubject>> logger
)
    : CommandRepository<MajorSubject>(dbContext, logger),
        IMajorSubjectCommandRepository { }