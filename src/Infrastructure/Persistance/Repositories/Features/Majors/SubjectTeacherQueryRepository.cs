using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class SubjectTeacherQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<SubjectTeacher>>          logger,
    IStringLocalizer<SubjectTeacher> localizer
)
    : QueryRepository<SubjectTeacher>(dbContext, logger),
        ISubjectTeacherQueryRepository;