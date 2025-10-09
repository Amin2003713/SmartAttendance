using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class SubjectTeacherCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<SubjectTeacher>> logger
)
    : CommandRepository<SubjectTeacher>(dbContext, logger),
        ISubjectTeacherCommandRepository { }