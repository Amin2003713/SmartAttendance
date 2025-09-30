using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class MajorTeacherQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<MajorTeacher>>          logger,
    IStringLocalizer<MajorTeacher> localizer
)
    : QueryRepository<MajorTeacher>(dbContext, logger),
        IMajorTeacherQueryRepository;