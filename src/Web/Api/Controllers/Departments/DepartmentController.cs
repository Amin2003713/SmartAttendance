using Microsoft.AspNetCore.Mvc;
using Shifty.Application.Features.Departments.Commands.Create;
using Shifty.Application.Features.Departments.Queries.GetDepartments;
using Shifty.Application.Features.Departments.Requests.Commands.Create;
using Shifty.Application.Features.Departments.Requests.Queries.GetDepartments;

namespace Shifty.Api.Controllers.Departments;

public class DepartmentController : IpaBaseController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a Department",
        Description = "Creates a new Department ")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task CreateDepartment([FromBody] CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateDepartmentCommand>(), cancellationToken);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get Departments")]
    [ProducesResponseType(typeof(List<GetDepartmentResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<List<GetDepartmentResponse>> GetDepartments(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetDepartmentsQuery(), cancellationToken);
    }
}