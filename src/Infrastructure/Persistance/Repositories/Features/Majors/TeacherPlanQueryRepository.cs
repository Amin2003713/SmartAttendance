using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class TeacherPlanQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<TeacherPlan>>          logger,
    IStringLocalizer<TeacherPlan> localizer
)
    : QueryRepository<TeacherPlan>(dbContext, logger),
        ITeacherPlanQueryRepository;