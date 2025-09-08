namespace SmartAttendance.Application.Features.Users.Requests.Commands.AddRole;

public class UpdateEmployeeRequest
{
    public List<string> Roles { get; set; }

    public Guid UserId { get; set; }
}