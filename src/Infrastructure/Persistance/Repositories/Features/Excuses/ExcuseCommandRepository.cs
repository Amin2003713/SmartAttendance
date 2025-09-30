using SmartAttendance.Application.Interfaces.Attendances;
using SmartAttendance.Application.Interfaces.Excuses;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.Excuses;

namespace SmartAttendance.Persistence.Repositories.Features.Excuses;

public class ExcuseCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<Excuse>> logger
)
    : CommandRepository<Excuse>(dbContext, logger),
        IExcuseCommandRepository { }