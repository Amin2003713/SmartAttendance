using Mapster;
using Microsoft.EntityFrameworkCore;
using Shifty.Application.Features.Departments.Queries.ById;
using Shifty.Application.Features.Departments.Requests.Queries.GetDepartments;
using Shifty.Application.Interfaces.Departments;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Features.Departments.Queries.ById;

public  class GetDepartmentByIdQueryHandler(
    IDepartmentQueryRepository departmentQueryRepository
) : IRequestHandler< GetDepartmentByIdQuery,  GetDepartmentResponse>
{
    public async Task<GetDepartmentResponse> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        if (! await departmentQueryRepository.AnyAsync(department => department.Id == request.Id  , cancellationToken))
            throw ShiftyException.NotFound("department not found");

        return (await departmentQueryRepository.TableNoTracking.Where(a => a.Id == request.Id).SingleOrDefaultAsync(cancellationToken: cancellationToken))
            .Adapt<GetDepartmentResponse>();
    }
}