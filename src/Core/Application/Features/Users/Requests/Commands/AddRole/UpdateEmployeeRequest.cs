using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Application.Features.Users.Requests.Commands.AddRole;

public class UpdateEmployeeRequest
{
    public UserType Type { get; set; }

    public Guid UserId { get; set; }
}