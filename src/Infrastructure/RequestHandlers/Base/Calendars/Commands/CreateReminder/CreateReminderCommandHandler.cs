using Mapster;
using Shifty.Application.Base.Calendars.Commands.CreateReminder;
using Shifty.Application.Interfaces.Calendars.CalendarProjects;
using Shifty.Application.Interfaces.Calendars.CalendarUsers;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Common.Exceptions;
using Shifty.Domain.Calenders.CalenderProjects;
using Shifty.Domain.Calenders.CalenderUsers;
using Shifty.Domain.Calenders.DailyCalender;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Base.Calendars.Commands.CreateReminder;

public class CreateReminderCommandHandler(
    IdentityService service,
    IDailyCalendarCommandRepository dailyCalendarCommandRepository,
    ICalendarProjectCommandRepository calendarProjectCommandRepository,
    ICalendarUserCommandRepository calendarUserCommandRepository,
    IMediator mediator,
    IStringLocalizer<CreateReminderCommandHandler> localizer,
    ILogger<CreateReminderCommandHandler> logger
)
    : IRequestHandler<CreateReminderCommand>
{
    public async Task Handle(CreateReminderCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the current user's identifier.
        var userId = service.GetUserId<Guid>();

        // Log the start of the command handling with detailed context.
        // logger.LogInformation(
        //     "User {UserId} is initiating a reminder creation for ProjectId: {ProjectId} at {Timestamp}.",
        //     userId,
        //     request.ProjectId,
        //     DateTime.UtcNow);


        // var access =
        //     await mediator.Send(new GetProjectUserQuery(request.ProjectId, userId, ApplicationConstant.ServiceName),
        //         cancellationToken);


        // // Check whether the user attempts to add reminders for other users when not allowed.
        // if (!access.AccessList.HasAccess((int)TenantAccess.ReminderAccess) &&
        //     request.TargetUsers.Any(uid => uid.Id != userId))
        // {
        //     logger.LogWarning("User {UserId} attempted to add a reminder for other users in ProjectId: {ProjectId}.",
        //         userId,
        //         request.ProjectId);
        //
        //     // Return a short, clear error message to the client.
        //     throw IpaException.Forbidden(localizer["Not allowed."].Value);
        // }


        try
        {
            List<Guid> targetUsers =
                // access.AccessList.HasAccess((int)TenantAccess.ReminderAccess)
                // ? request.TargetUsers.Select(uid => uid.Id).ToList()
                // :
                [userId];

            // Ensure the current user is included.
            if (!targetUsers.Contains(userId)) targetUsers.Add(userId);

            // Map the request to a DailyCalendar entity configured as a reminder.
            var reminderDailyCalendar = request.Adapt<DailyCalendar>();
            reminderDailyCalendar.IsReminder = true;
            await dailyCalendarCommandRepository.AddAsync(reminderDailyCalendar, cancellationToken);

            // Map the request to a CalendarProject entity and link it to the created DailyCalendar.
            var reminderProject = request.Adapt<CalendarProject>();
            reminderProject.CalendarId = reminderDailyCalendar.Id;
            await calendarProjectCommandRepository.AddAsync(reminderProject, cancellationToken);


            var reminderUsers = targetUsers.Select(a => new CalendarUser
                {
                    CalendarId = reminderDailyCalendar.Id,
                    UserId = a
                })
                .ToList();

            await calendarUserCommandRepository.AddRangeAsync(reminderUsers, cancellationToken);

            // logger.LogInformation(
            //     "Reminder created successfully with CalendarId: {CalendarId} for ProjectId: {ProjectId}.",
            //     reminderDailyCalendar.Id,
            //     request.ProjectId);
        }
        catch (Exception ex)
        {
            // Log detailed error information and context.
            // logger.LogError(ex,
            //     "Error creating reminder in ProjectId: {ProjectId} for UserId: {UserId}. Error details: {ErrorMessage}",
            //     request.ProjectId,
            //     userId,
            //     ex.Message);

            // Throw a short, descriptive error message to the client.
            throw IpaException.InternalServerError(localizer["Reminder creation failed."].Value);
        }
    }
}