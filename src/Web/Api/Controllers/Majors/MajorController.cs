using System.Linq;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SmartAttendance.Application.Features.Majors.Commands;
using SmartAttendance.Application.Features.Majors.Commands.Create;
using SmartAttendance.Application.Features.Majors.Commands.DeActive;
using SmartAttendance.Application.Features.Majors.Queries;
using SmartAttendance.Application.Features.Majors.Queries.GetAll;
using SmartAttendance.Application.Features.Majors.Queries.GetById;
using SmartAttendance.Application.Features.Majors.Requests;
using SmartAttendance.Application.Features.Majors.Requests.Create;
using SmartAttendance.Application.Features.Majors.Responses;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.Api.Controllers.Majors;

public class MajorController : SmartAttendanceBaseController
{
    /// <summary>
    /// Creates a new subject.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreateMajor([FromBody] CreateMajorRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateMajorCommand>(), cancellationToken);
    }

    /// <summary>
    /// Deletes a subject by its ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task DeleteMajor(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeActiveMajorCommand()
            {
                Id = id
            } ,
            cancellationToken);
    }

    /// <summary>
    /// Gets all subjects.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<GetMajorInfoResponse>), StatusCodes.Status200OK)]
    public async Task<List<GetMajorInfoResponse>> GetAllMajors(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetAllMajorQuery(), cancellationToken);
    }

    /// <summary>
    /// Gets a subject by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetMajorInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetMajorInfoResponse> GetMajorById(Guid id, CancellationToken cancellationToken)
    {
        return (await Mediator.Send(new GetMajorById(id),
            cancellationToken));
    }
}