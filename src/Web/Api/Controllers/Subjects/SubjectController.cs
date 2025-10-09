using System.Linq;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SmartAttendance.Application.Features.Subjects.Commands;
using SmartAttendance.Application.Features.Subjects.Commands.Create;
using SmartAttendance.Application.Features.Subjects.Commands.Delete;
using SmartAttendance.Application.Features.Subjects.Queries;
using SmartAttendance.Application.Features.Subjects.Queries.ByIds;
using SmartAttendance.Application.Features.Subjects.Queries.GetAll;
using SmartAttendance.Application.Features.Subjects.Queries.GetSubjectsByMajor;
using SmartAttendance.Application.Features.Subjects.Queries.GetTeacherSubjects;
using SmartAttendance.Application.Features.Subjects.Requests;
using SmartAttendance.Application.Features.Subjects.Responses;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.Api.Controllers.Subjects;

[Route("api/[controller]")]
[ApiController]
public class SubjectController : SmartAttendanceBaseController
{
    /// <summary>
    /// Creates a new subject.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreateSubject([FromBody] CreateSubjectRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateSubjectCommand>(), cancellationToken);
    }

    /// <summary>
    /// Deletes a subject by its ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task DeleteSubject(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteSubjectCommand(id) , cancellationToken);
    }

    /// <summary>
    /// Gets all subjects.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<GetSubjectInfoResponse>), StatusCodes.Status200OK)]
    public async Task<List<GetSubjectInfoResponse>> GetAllSubjects(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetAllSubjectsQuery(), cancellationToken);
    }

    /// <summary>
    /// Gets a subject by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetSubjectInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetSubjectInfoResponse> GetSubjectById(Guid id, CancellationToken cancellationToken)
    {
        return (await Mediator.Send(new GetSubjectByIdsQuery([id]),
            cancellationToken)).FirstOrDefault();
    }

    /// <summary>
    /// Gets subjects for a specific major.
    /// </summary>
    [HttpGet("major/{majorId:guid}")]
    [ProducesResponseType(typeof(List<GetSubjectInfoResponse>), StatusCodes.Status200OK)]
    public async Task<List<GetSubjectInfoResponse>> GetSubjectsForMajor(Guid majorId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetSubjectsByMajorQuery(majorId),
            cancellationToken);
    }

    /// <summary>
    /// Gets subjects assigned to a specific teacher.
    /// </summary>
    [HttpGet("teacher/{teacherId:guid}")]
    [ProducesResponseType(typeof(List<GetSubjectInfoResponse>), StatusCodes.Status200OK)]
    public async Task<List<GetSubjectInfoResponse>> GetTeacherSubjects(Guid teacherId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetTeacherSubjectsQuery(teacherId),
            cancellationToken);
    }
}