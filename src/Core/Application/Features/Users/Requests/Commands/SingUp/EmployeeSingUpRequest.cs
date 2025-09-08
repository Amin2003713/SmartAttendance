namespace SmartAttendance.Application.Features.Users.Requests.Commands.SingUp;

public class EmployeeSingUpRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public List<string> RolesList { get; set; }

    public string Address { get; set; }
    public Guid? DivisionId { get; set; }
}