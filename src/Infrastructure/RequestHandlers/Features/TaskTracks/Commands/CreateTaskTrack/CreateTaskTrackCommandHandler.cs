using Mapster;
using SmartAttendance.Application.Features.TaskTrack.Commands.CreateTaskTrack;
using SmartAttendance.Application.Features.Users.Queries.GetAllUsers;
using SmartAttendance.Application.Interfaces.Base.EventInterface;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.TaskTracks.Aggregate;
using SmartAttendance.Domain.TaskTracks.Events.TaskTrackers;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.TaskTracks.Commands.CreateTaskTrack;

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

                throw SmartAttendanceException.Forbidden(
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
        catch (SmartAttendanceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message,
                "Error creating TaskTracker via ES .");

            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while creating the taskTracker."].Value);
        }
    }
}