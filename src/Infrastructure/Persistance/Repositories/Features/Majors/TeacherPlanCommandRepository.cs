using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class TeacherPlanCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<TeacherPlan>> logger
)
    : CommandRepository<TeacherPlan>(dbContext, logger),
        ITeacherPlanCommandRepository { }