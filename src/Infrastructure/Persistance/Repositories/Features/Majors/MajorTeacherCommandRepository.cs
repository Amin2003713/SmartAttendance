using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class MajorTeacherCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<MajorTeacher>> logger
)
    : CommandRepository<MajorTeacher>(dbContext, logger),
        IMajorTeacherCommandRepository { }