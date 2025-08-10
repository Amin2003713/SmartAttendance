using Shifty.Application.Interfaces.Stations;
using Shifty.Domain.Stations;

namespace Shifty.Persistence.Stations;

public class StationQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Station>> logger
)
    : QueryRepository<Station>(dbContext, logger),
        IStationQueryRepository;