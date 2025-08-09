using Shifty.Domain.Departments;

namespace Shifty.Application.Departments.Requests.Commands.Create;

public class CreateDepartmentRequest
{
    public string Title { get; set; }

    public Guid? ParentDepartmentId { get; set; }

    public Guid? ManagerId { get; set; }
}