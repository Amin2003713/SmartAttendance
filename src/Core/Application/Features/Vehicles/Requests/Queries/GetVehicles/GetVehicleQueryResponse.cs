using SmartAttendance.Common.Common.Requests;
using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Application.Features.Vehicles.Requests.Queries.GetVehicles;

public class GetVehicleQueryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public PlateNumber PlateNumber { get; set; }

    public VehicleType VehicleType { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public DateTime YearOfProduction { get; set; }
    public LogPropertyInfoResponse Responsible { get; set; }
    public string Descriprion { get; set; }
    public VehicleStatus Status { get; set; }
}