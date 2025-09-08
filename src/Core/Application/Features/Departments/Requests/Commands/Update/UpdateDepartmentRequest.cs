using SmartAttendance.Application.Features.Departments.Requests.Commands.Create;

namespace SmartAttendance.Application.Features.Departments.Requests.Commands.Update;

public class UpdateDepartmentRequest : CreateDepartmentRequest
{
    public Guid Id { get; set; }
}