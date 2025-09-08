using SmartAttendance.Application.Interfaces.Stations;
using SmartAttendance.Domain.Stations;

namespace SmartAttendance.Persistence.Repositories.Stations;

public class StationCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Station>> logger
)
    : CommandRepository<Station>(dbContext, logger),
        IStationCommandRepository;