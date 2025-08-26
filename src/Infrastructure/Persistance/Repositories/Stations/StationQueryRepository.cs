using Shifty.Application.Interfaces.Stations;
using Shifty.Domain.Stations;

namespace Shifty.Persistence.Repositories.Stations;

public class StationQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Station>> logger
)
    : QueryRepository<Station>(dbContext, logger),
        IStationQueryRepository;