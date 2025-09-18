using SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;

namespace SmartAttendance.Application.Features.Users.Requests.Commands.AddRole;

public class UpdateEmployeeRequest  : RegisterByOwnerRequest
{
    public Guid UserId { get; set; }
}