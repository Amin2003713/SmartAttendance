using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Persistence.Repositories.Features.Majors;

public class MajorCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<Major>> logger
)
    : CommandRepository<Major>(dbContext, logger),
        IMajorCommandRepository { }