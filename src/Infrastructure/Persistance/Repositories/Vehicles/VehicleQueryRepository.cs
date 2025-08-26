using Shifty.Application.Interfaces.Vehicles;
using Shifty.Domain.Vehicles;

namespace Shifty.Persistence.Repositories.Vehicles;

public class VehicleQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Vehicle>> logger
)
    : QueryRepository<Vehicle>(dbContext, logger),
        IVehicleQueryRepository;