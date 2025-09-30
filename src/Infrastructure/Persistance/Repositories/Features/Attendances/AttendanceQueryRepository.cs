using SmartAttendance.Application.Interfaces.Attendances;
using SmartAttendance.Domain.Features.Attendances;

namespace SmartAttendance.Persistence.Repositories.Features.Attendances;

public class AttendanceQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<Attendance>>          logger,
    IStringLocalizer<Attendance> localizer
)
    : QueryRepository<Attendance>(dbContext, logger),
        IAttendanceQueryRepository;