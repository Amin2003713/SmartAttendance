using SmartAttendance.Application.Interfaces.Subjects;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Subjects;

public class TeacherPlanQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<TeacherPlan>>          logger,
    IStringLocalizer<TeacherPlan> localizer
)
    : QueryRepository<TeacherPlan>(dbContext, logger),
        ITeacherPlanQueryRepository;