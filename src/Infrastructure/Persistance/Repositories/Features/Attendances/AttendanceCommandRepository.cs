using SmartAttendance.Application.Interfaces.Attendances;
using SmartAttendance.Domain.Features.Attendances;

namespace SmartAttendance.Persistence.Repositories.Features.Attendances;



public class AttendanceCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<Attendance>> logger
)
    : CommandRepository<Attendance>(dbContext, logger),
        IAttendanceCommandRepository { }