using SmartAttendance.Application.Interfaces.Vehicles;
using SmartAttendance.Domain.Vehicles;

namespace SmartAttendance.Persistence.Repositories.Vehicles;

public class VehicleQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Vehicle>> logger
)
    : QueryRepository<Vehicle>(dbContext, logger),
        IVehicleQueryRepository;