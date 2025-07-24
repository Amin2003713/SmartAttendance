using Shifty.Application.Settings.Commands.UpdateSetting;
using Shifty.Application.Settings.Requests.UpdateSetting;

namespace Shifty.Api.Controllers.Settings;

public class SettingController : IpaBaseController
{
    /// <summary>
    ///     Updates the company settings.
    /// </summary>
    /// <param name="request">The request containing updated setting values.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="204">Settings updated successfully.</response>
    /// <response code="400">Invalid input data.</response>
    /// <response code="401">Unauthorized to perform this action.</response>
    /// <response code="404">Setting not found.</response>
    [HttpPut("update")]
    [SwaggerOperation(Summary = "Update Settings", Description = "Updates company-level settings.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound)]
    public virtual async Task Update([FromBody] UpdateSettingRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateSettingCommand>(), cancellationToken);
    }
}