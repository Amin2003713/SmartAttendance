using SmartAttendance.Application.Interfaces.Subjects;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Subjects;

public class SubjectTeacherCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<SubjectTeacher>> logger
)
    : CommandRepository<SubjectTeacher>(dbContext, logger),
        ISubjectTeacherCommandRepository { }