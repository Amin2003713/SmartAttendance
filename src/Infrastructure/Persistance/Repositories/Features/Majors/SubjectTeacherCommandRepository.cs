using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class SubjectTeacherCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<SubjectTeacher>> logger
)
    : CommandRepository<SubjectTeacher>(dbContext, logger),
        ISubjectTeacherCommandRepository { }


public class MajorCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<Major>> logger
)
    : CommandRepository<Major>(dbContext, logger),
        IMajorCommandRepository { }
public class MajorSubjectCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<MajorSubject>> logger
)
    : CommandRepository<MajorSubject>(dbContext, logger),
        IMajorSubjectCommandRepository { }