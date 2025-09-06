using Mapster;
using Shifty.Application.Features.TaskTrack.Commands.UpdateTaskTrack;
using Shifty.Application.Features.Users.Queries.GetAllUsers;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Exceptions;
using Shifty.Domain.TaskTracks.Aggregate;
using Shifty.Domain.TaskTracks.Events.TaskTrackers;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.TaskTracks.Commands.UpdateTaskTrack;

public class UpdateTaskTrackCommandHandler(
    IEventReader<TaskTrack, Guid> eventReader,
    IEventWriter<TaskTrack, Guid> eventWriter,
    ILogger<UpdateTaskTrackCommandHandler> logger,
    IStringLocalizer<UpdateTaskTrackCommandHandler> localizer,
    IdentityService identityService,
    IMediator mediator
) : IRequestHandler<UpdateTaskTrackCommand>
{
    public async Task Handle(UpdateTaskTrackCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating taskTrack {TaskTrackId} .",
            request.AggregateId);

        if (!await eventReader.ExistsAsync(request.AggregateId, cancellationToken))
        {
            logger.LogWarning("Missions {TackTrackId} not found.", request.AggregateId);
            throw ShiftyException.NotFound(localizer["Missions not found."].Value);
        }

        var userId = identityService.GetUserId<Guid>();


        var taskTrack = await eventReader.GetSingleAsync(
            x => x.AggregateId == request.AggregateId && x.UserId == identityService.GetUserId<Guid>(),
            cancellationToken: cancellationToken);


        var users = mediator.Send(new GetAllUsersQuery(), cancellationToken);

        var projectMemberIds = users.Result.Select(u => u.Id).ToHashSet();


        if (request.AssigneeId is not null &&
            !request.AssigneeId.SequenceEqual(taskTrack!.AssigneeId))
        {
            var invalidUsers = request.AssigneeId.Where(u => !projectMemberIds.Contains(u)).ToList();

            if (invalidUsers.Count != 0)
            {
                logger.LogWarning("The following users are not members : {Users}",
                    string.Join(",", invalidUsers));

                throw ShiftyException.Forbidden(
                    localizer["One or more assigned users are not members of the selected project."].Value,
                    new
                    {
                        InvalidUsers = invalidUsers
                    });
            }
        }

        taskTrack!.Update(request.Adapt<TaskTrackUpdatedEvent>() with
        {
            AggregateId = request.AggregateId,
            CreatedBy = userId,
            Reported = DateTime.Now
        });


        try
        {
            await eventWriter.SaveAsync(taskTrack, cancellationToken);
            logger.LogInformation("Missions {taskTrackId} updated successfully.", request.AggregateId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating taskTrack {TaskTrackId}.", request.AggregateId);
            throw;
        }
    }
}