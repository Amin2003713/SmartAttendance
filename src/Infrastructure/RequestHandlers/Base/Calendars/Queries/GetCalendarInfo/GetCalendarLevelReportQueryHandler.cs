// using Mapster;
// using Shifty.Application.Calendars.Queries.GetCalendarLevelReport;
// using Shifty.Application.Calendars.Request.Queries.GetCalendarLevelReport;
// using Shifty.Common.Exceptions;
// using Shifty.Common.General;
// using Shifty.Common.General.Enums.Projects;
// using Shifty.Common.General.Enums.Workflows;
// using Shifty.Persistence.Services.Identities;
//
// namespace Shifty.RequestHandlers.Calendars.Queries.GetCalendarInfo;
//
// public class GetCalendarLevelReportQueryHandler(
//     IMediator mediator,
//     IdentityService identityService
// )
//     : IRequestHandler<GetCalendarLevelReportQuery, List<CalendarInfoForLevelResponse>>
// {
//     public async Task<List<CalendarInfoForLevelResponse>> Handle(
//         GetCalendarLevelReportQuery request,
//         CancellationToken cancellationToken)
//     {
//         var result = new List<CalendarInfoForLevelResponse>();
//
//         var userId = identityService.GetUserId<Guid>();
//
//         var access =
//             await mediator.Send(new GetProjectUserQuery(request.ProjectId, userId, ApplicationConstant.ServiceName),
//                 cancellationToken);
//
//
//         if (access == null)
//             throw ShiftyException.Forbidden("Access denied");
//
//         var currentLevel = access.Node;
//
//         var fetchTasks
//             = await broker.RequestAsync<GetPrimaCalendarLevelReportBrokerResponse, GetPrimaCalendarLevelReportBroker>(
//                 new GetPrimaCalendarLevelReportBroker
//                 {
//                     Access = access.Adapt<GetProjectUserAccessBrokerResponse>(),
//
//                     Date = request.Date
//                 },
//                 cancellationToken);
//
//
//         foreach (var kvp in fetchTasks.Result.reviewActions)
//         {
//             try
//             {
//                 var statuses = kvp.Value.ToList();
//
//                 if (statuses.Count > 0)
//                     result.Add(new CalendarInfoForLevelResponse
//                     {
//                         Data = ComputeBadge(statuses, currentLevel),
//                         ItemName = kvp.Key.GetEnglishName(),
//                         Type = kvp.Key,
//                         ItemNamePersian = kvp.Key.GetDisplayName()
//                     });
//             }
//             catch
//             {
//                 // ignored
//             }
//         }
//
//         return result;
//     }
//
//
//     private static OrderedDictionary<UserType, string> ComputeBadge(List<IDictionary<UserType, ReviewAction>> allReviewStatuses, UserType node)
//     {
//         if (allReviewStatuses == null)
//             return null!;
//
//         var userLevelRatios = allReviewStatuses
//             // Flatten all dictionaries into a sequence of (UserType, ReviewAction) pairs
//             .SelectMany(dict => dict)
//             // Group by UserType (i.e. userLevel)
//             .GroupBy(kvp => kvp.Key)
//             .Select(group =>
//             {
//                 // Total number of entries for this UserType
//                 var total = group.Count();
//
//                 // Count how many of those entries have ReviewAction == Verify
//                 var verified = group.Count(kvp => kvp.Value == ReviewAction.Verify);
//
//                 // Return a tuple (or anonymous object, or any structure you need)
//                 return new
//                 {
//                     node = group.Key,
//                     Ratio = $"{verified}/{total}"
//                 };
//             })
//             .ToList();
//
//
//         return new OrderedDictionary<UserType, string>(userLevelRatios.Where(a => a.node <= node)
//             .ToDictionary(a => a.node, a => a.Ratio));
//     }
// }
//
