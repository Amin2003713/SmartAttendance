using Shifty.Application.Departments.Requests.Commands.Create;

namespace Shifty.Application.Departments.Requests.Commands.Update;

public class UpdateDepartmentRequest : CreateDepartmentRequest
{
    public Guid Id { get; set; }
}