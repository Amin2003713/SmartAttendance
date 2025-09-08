namespace SmartAttendance.Application.Features.Departments.Requests.Queries.GetDepartments;

public class GetDepartmentResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ParentTitle { get; set; }
    public string? ManagerName { get; set; }
}