// using Mapster;
// using Shifty.Application.Users.Queries.GetAllUsers;
//
// namespace Shifty.RequestHandlers.Dashboards.Queries.GetRecentActivities;
//
// public class GetRecentActivitiesQueryHandler(
//     IMediator mediator
// ) :
//     IRequestHandler<GetRecentActivitiesQuery, List<GetResentActivities>>
// {
//     public async Task<List<GetResentActivities>> Handle(
//         GetRecentActivitiesQuery request,
//         CancellationToken cancellationToken)
//     {
//         try
//         {
//             var projects = request.ProjectId is null
//                 ? await mediator.Send(new GetAllProjectQuery(), cancellationToken)
//                 :
//                 [
//                     (await mediator.Send(new GetProjectByIdQuery(request.ProjectId.Value), cancellationToken))
//                     .Adapt<GetProjectQueryResponse>()
//                 ];
//
//             if (!projects.Any())
//                 return new List<GetResentActivities>();
//
//             var access = await mediator.Send(new ListLoginUserProjectsAccessQuery(), cancellationToken);
//             var users  = await mediator.Send(new GetAllUsersQuery(),                 cancellationToken);
//
//             var prima = await broker.RequestAsync<GetResentActivitiesBrokerResponse, GetRecentActivitiesBroker>(
//                 request.Adapt<GetRecentActivitiesBroker>() with
//                 {
//                     Access = access.Adapt<List<GetProjectUserAccessBrokerResponse>>(),
//                     Projects = projects.Select(p => p.Id).ToList()
//                 },
//                 cancellationToken
//             );
//
//             var activityList = new List<GetResentActivities>();
//
//             foreach (var (projectId, activitiesByEventId) in prima.Response)
//             {
//                 var projectName = projects.SingleOrDefault(p => p.Id == projectId)?.Name ?? projectId.ToString();
//
//                 foreach (var (_, activity) in activitiesByEventId)
//                 {
//                     var userId = Guid.TryParse(activity.User, out var parsedUserId) ? parsedUserId : Guid.Empty;
//                     var user   = users.SingleOrDefault(u => u.Id == userId);
//
//                     var updatedActivity = activity with
//                     {
//                         Project = projectName,
//                         User = user is not null ? $"{user.FirstName} {user.LastName}" : activity.User,
//                         Changes = activity.Changes
//                             .Select(change =>
//                             {
//                                 var modifierId = Guid.TryParse(change.ModifiedBy, out var parsedModifierId)
//                                     ? parsedModifierId
//                                     : Guid.Empty;
//
//                                 var modifier = users.SingleOrDefault(u => u.Id == modifierId);
//
//                                 return change with
//                                 {
//                                     ModifiedBy = modifier is not null
//                                         ? $"{modifier.FirstName} {modifier.LastName}"
//                                         : change.ModifiedBy
//                                 };
//                             })
//                             .ToList()
//                     };
//
//
//                     activityList.Add(updatedActivity);
//                 }
//             }
//
//             return activityList.OrderByDescending(a => a.OnDate).Take(request.TopNRecords).ToList();
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             throw;
//         }
//     }
// }

