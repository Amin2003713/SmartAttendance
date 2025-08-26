using Shifty.Application.Features.Departments.Requests.Commands.Create;

namespace Shifty.Application.Features.Departments.Requests.Commands.Update;

public class UpdateDepartmentRequest : CreateDepartmentRequest
{
    public Guid Id { get; set; }
}