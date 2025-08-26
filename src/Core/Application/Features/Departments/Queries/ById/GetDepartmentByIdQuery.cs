using Shifty.Application.Features.Departments.Requests.Queries.GetDepartments;

namespace Shifty.Application.Features.Departments.Queries.ById;

public record GetDepartmentByIdQuery(Guid Id) : IRequest<GetDepartmentResponse>;