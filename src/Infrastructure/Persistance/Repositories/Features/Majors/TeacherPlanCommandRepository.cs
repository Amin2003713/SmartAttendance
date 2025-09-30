using SmartAttendance.Application.Interfaces.Subjects;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Repositories.Features.Subjects;

public class TeacherPlanCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<TeacherPlan>> logger
)
    : CommandRepository<TeacherPlan>(dbContext, logger),
        ITeacherPlanCommandRepository { }