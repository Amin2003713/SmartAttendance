using Shifty.Application.Interfaces.Vehicles;
using Shifty.Domain.Vehicles;

namespace Shifty.Persistence.Repositories.Vehicles;

public class VehicleCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Vehicle>> logger
)
    : CommandRepository<Vehicle>(dbContext, logger),
        IVehicleCommandRepository;