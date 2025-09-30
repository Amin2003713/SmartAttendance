using SmartAttendance.Application.Interfaces.Subjects;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Subjects;

public class SubjectTeacherQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<SubjectTeacher>>          logger,
    IStringLocalizer<SubjectTeacher> localizer
)
    : QueryRepository<SubjectTeacher>(dbContext, logger),
        ISubjectTeacherQueryRepository;