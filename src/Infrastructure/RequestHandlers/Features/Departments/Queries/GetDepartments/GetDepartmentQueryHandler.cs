using Microsoft.EntityFrameworkCore;
using Shifty.Application.Features.Departments.Queries.GetDepartments;
using Shifty.Application.Features.Departments.Requests.Queries.GetDepartments;
using Shifty.Application.Interfaces.Departments;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Features.Departments.Queries.GetDepartments;

public class GetDepartmentQueryHandler(
    IDepartmentQueryRepository departmentQueryRepository,
    ILogger<GetDepartmentQueryHandler> logger,
    IStringLocalizer<GetDepartmentQueryHandler> localizer)
    : IRequestHandler<GetDepartmentsQuery, List<GetDepartmentResponse>>
{
    public async Task<List<GetDepartmentResponse>> Handle(GetDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var listDepartments = await departmentQueryRepository.TableNoTracking
                .Where(d => d.IsActive && d.DeletedBy == null)
                .Include(d => d.ParentDepartment)
                .Include(d => d.Manager)
                .AsSplitQuery()
                .OrderBy(d => d.CreatedAt)
                .Select(d => new GetDepartmentResponse
                {
                    Id = d.Id,
                    Title = d.Title,
                    ParentTitle = (d.ParentDepartmentId != null ? d.ParentDepartment!.Title : null)!,
                    ManagerName = d.ManagerId != null
                        ? $"{d.Manager!.FullName()}"
                        : null
                })
                .ToListAsync(cancellationToken);

            if (listDepartments.Count == 0)
            {
                logger.LogInformation("No departments found (returning empty list).");
                return [];
            }

            logger.LogInformation("Retrieved {Count} departments.", listDepartments.Count);
            return listDepartments;
        }
        catch (ShiftyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving departments.");
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while retrieving departments."]);
        }
    }
}