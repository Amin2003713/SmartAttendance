using Shifty.Application.Departments.Requests.Queries.GetDepartments;
using Shifty.Domain.Departments;

namespace Shifty.Application.Departments.Queries.GetDepartments;

public class GetDepartmentsQuery : IRequest<List<GetDepartmentResponse>>;