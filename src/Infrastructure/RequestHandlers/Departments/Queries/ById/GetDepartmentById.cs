using Mapster;
using Microsoft.EntityFrameworkCore;
using Shifty.Application.Departments.Queries.ById;
using Shifty.Application.Departments.Requests.Queries.GetDepartments;
using Shifty.Application.Interfaces.Departments;
using Shifty.Application.Stations.Requests.Queries.GetStations;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Departments.Queries.ById;

public  class GetDepartmentByIdQueryHandler(
    IDepartmentQueryRepository departmentQueryRepository
) : IRequestHandler< GetDepartmentByIdQuery,  GetDepartmentResponse>
{
    public async Task<GetDepartmentResponse> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        if (! await departmentQueryRepository.AnyAsync(department => department.Id == request.Id  , cancellationToken))
            throw IpaException.NotFound("department not found");

        return (await departmentQueryRepository.TableNoTracking.Where(a => a.Id == request.Id).SingleOrDefaultAsync(cancellationToken: cancellationToken))
            .Adapt<GetDepartmentResponse>();
    }
}