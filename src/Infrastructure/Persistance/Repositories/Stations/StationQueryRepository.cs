using SmartAttendance.Application.Interfaces.Stations;
using SmartAttendance.Domain.Stations;

namespace SmartAttendance.Persistence.Repositories.Stations;

public class StationQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Station>> logger
)
    : QueryRepository<Station>(dbContext, logger),
        IStationQueryRepository;