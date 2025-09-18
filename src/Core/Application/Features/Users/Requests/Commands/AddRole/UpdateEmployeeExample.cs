namespace SmartAttendance.Application.Features.Users.Requests.Commands.AddRole;

public class UpdateEmployeeExample : IExamplesProvider<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequest GetExamples()
    {
        return new UpdateEmployeeRequest();
    }
}