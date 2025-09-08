using SmartAttendance.Application.Features.Departments.Requests.Queries.GetDepartments;

namespace SmartAttendance.Application.Features.Departments.Queries.ById;

public record GetDepartmentByIdQuery(
    Guid Id
) : IRequest<GetDepartmentResponse>;