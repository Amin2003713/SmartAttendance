using SmartAttendance.Application.Interfaces.Vehicles;
using SmartAttendance.Domain.Vehicles;

namespace SmartAttendance.Persistence.Repositories.Vehicles;

public class VehicleCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Vehicle>> logger
)
    : CommandRepository<Vehicle>(dbContext, logger),
        IVehicleCommandRepository;