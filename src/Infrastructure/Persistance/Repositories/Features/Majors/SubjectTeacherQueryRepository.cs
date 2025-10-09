using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class SubjectTeacherQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<SubjectTeacher>>          logger,
    IStringLocalizer<SubjectTeacher> localizer
)
    : QueryRepository<SubjectTeacher>(dbContext, logger),
        ISubjectTeacherQueryRepository;

public class MajorQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<Major>>          logger,
    IStringLocalizer<Major> localizer
)
    : QueryRepository<Major>(dbContext, logger),
        IMajorQueryRepository;

public class MajorSubjectQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<MajorSubject>>          logger,
    IStringLocalizer<MajorSubject> localizer
)
    : QueryRepository<MajorSubject>(dbContext, logger),
        IMajorSubjectQueryRepository;