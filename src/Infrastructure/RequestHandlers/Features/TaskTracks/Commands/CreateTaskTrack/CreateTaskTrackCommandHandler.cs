using Mapster;
using Shifty.Application.Features.TaskTrack.Commands.CreateTaskTrack;
using Shifty.Application.Features.Users.Queries.GetAllUsers;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Exceptions;
using Shifty.Domain.TaskTracks.Aggregate;
using Shifty.Domain.TaskTracks.Events.TaskTrackers;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.TaskTracks.Commands.CreateTaskTrack;

public class CreateTaskTrackCommandHandler(
    ILogger<CreateTaskTrackCommandHandler> logger,
    IEventWriter<TaskTrack, Guid> eventWriter,
    IMediator mediator,
    IStringLocalizer<CreateTaskTrackCommandHandler> localizer,
    IdentityService identityService
)
    : IRequestHandler<CreateTaskTrackCommand>
{
    public async Task Handle(CreateTaskTrackCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating TaskTracking.");

        var userId = identityService.GetUserId<Guid>();


        var users = mediator.Send(new GetAllUsersQuery(), cancellationToken);

        var projectMemberIds = users.Result.Select(u => u.Id).ToHashSet();

        if (request.AssigneeId is not null && request.AssigneeId.Count != 0)
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

        try
        {
            var taskTracking = TaskTrack.New(request.Adapt<TaskTrackCreatedEvent>()with
            {
                AggregateId = Guid.CreateVersion7(DateTimeOffset.Now),
                CreatedBy = userId,
                Reported = DateTime.Now
            });

            await eventWriter.SaveAsync(taskTracking, cancellationToken);

            logger.LogInformation("TaskTrackersES {TaskTrackerId} created via event sourcing.",
                taskTracking.AggregateId);
        }
        catch (ShiftyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message,
                "Error creating TaskTracker via ES .");

            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while creating the taskTracker."].Value);
        }
    }
}