using Shifty.Application.Base.Storage.Queries.GetAllRemainStorage;
using Shifty.Application.Base.Storage.Request.Queries.GetRemainStorage;

namespace Shifty.Api.Controllers.Storages;

public class StorageController : ShiftyBaseController
{
    /// <summary>
    ///     Retrieves remaining storage across all projects.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Returns total and remaining storage information.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpGet("Get-Storages")]
    [SwaggerOperation(Summary = "Get All Storage Usage",
        Description = "Gets remaining storage information for all projects.")]
    [ProducesResponseType(typeof(GetRemainStorageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),        StatusCodes.Status401Unauthorized)]
    public async Task<GetRemainStorageResponse> GetRemainStorage(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetAllRemainStorageQuery(), cancellationToken);
    }

    // /// <summary>
    // ///     Retrieves remaining storage for a specific project.
    // /// </summary>
    // /// <param name="projectId">The ID of the project to check storage for.</param>
    // /// <param name="cancellationToken">Cancellation token for the request.</param>
    // /// <response code="200">Returns project-specific remaining storage.</response>
    // /// <response code="400">If the project ID is invalid.</response>
    // /// <response code="404">If the project is not found.</response>
    // [HttpGet("Get-Storage")]
    // [SwaggerOperation(Summary = "Get Projects Storage", Description = "Gets remaining storage for a specific project.")]
    // [ProducesResponseType(typeof(GetRemainStorageByProjectResponse), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ApiProblemDetails),                 StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(typeof(ApiProblemDetails),                 StatusCodes.Status404NotFound)]
    // public async Task<GetRemainStorageByProjectResponse> GetRemainStorageByProjectId(
    //     
    //     CancellationToken cancellationToken)
    // {
    //   todo:  return await Mediator.Send(new GetRemainStorageByProjectQuery(projectId), cancellationToken);
    // }
}