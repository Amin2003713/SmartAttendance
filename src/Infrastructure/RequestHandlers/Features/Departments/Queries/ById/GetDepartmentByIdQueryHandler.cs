using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Departments.Queries.ById;
using SmartAttendance.Application.Features.Departments.Requests.Queries.GetDepartments;
using SmartAttendance.Application.Interfaces.Departments;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Departments.Queries.ById;

public  class GetDepartmentByIdQueryHandler(
    IDepartmentQueryRepository departmentQueryRepository
) : IRequestHandler< GetDepartmentByIdQuery,  GetDepartmentResponse>
{
    public async Task<GetDepartmentResponse> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        if (! await departmentQueryRepository.AnyAsync(department => department.Id == request.Id  , cancellationToken))
            throw SmartAttendanceException.NotFound("department not found");

        return (await departmentQueryRepository.TableNoTracking.Where(a => a.Id == request.Id).SingleOrDefaultAsync(cancellationToken: cancellationToken))
            .Adapt<GetDepartmentResponse>();
    }
}