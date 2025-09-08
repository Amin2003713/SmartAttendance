namespace SmartAttendance.Application.Features.Vehicles.Commands.Delete;

public record DeleteVehicleCommand(
    Guid Id
) : IRequest;