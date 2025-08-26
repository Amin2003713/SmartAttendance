using Shifty.Application.Departments.Requests.Queries.GetDepartments;
using Shifty.Domain.Departments;

namespace Shifty.Application.Departments.Queries.ById;

public record GetDepartmentByIdQuery(Guid Id) : IRequest<GetDepartmentResponse>;