using Shifty.Application.Interfaces.Stations;
using Shifty.Domain.Stations;

namespace Shifty.Persistence.Repositories.Stations;

public class StationCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Station>> logger
)
    : CommandRepository<Station>(dbContext, logger),
        IStationCommandRepository;