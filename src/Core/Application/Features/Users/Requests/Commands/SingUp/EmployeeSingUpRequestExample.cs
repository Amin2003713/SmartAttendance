namespace SmartAttendance.Application.Features.Users.Requests.Commands.SingUp;

public class EmployeeSingUpRequestExample : IExamplesProvider<EmployeeSingUpRequest>
{
    public EmployeeSingUpRequest GetExamples()
    {
        return new EmployeeSingUpRequest
        {
            FirstName   = "John",
            LastName    = "Doe",
            PhoneNumber = "09134041409",
            RolesList   = ["Admin", "Driver"], // Example roles
            Address     = "123 Main St, City, Country",
            DivisionId  = Guid.CreateVersion7() // Example department ID
        };
    }
}