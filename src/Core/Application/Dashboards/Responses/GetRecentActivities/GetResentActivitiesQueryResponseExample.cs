// using Shifty.Common.Common.Responses.PropertyChanges;
// using Shifty.Common.Messaging.Contracts.Tenants.Dashboard.Queries.GetResentActivity;
// using Swashbuckle.AspNetCore.Filters;
//
// namespace Shifty.Application.Dashboards.Responses.GetRecentActivities;
//
// public class GetResentActivitiesQueryResponseExample : IExamplesProvider<List<GetResentActivities>>
// {
//     public List<GetResentActivities> GetExamples()
//     {
//         return new List<GetResentActivities>(new List<GetResentActivities>
//         {
//             new(
//                 "Project Alpha",
//                 "Ali Rezaei",
//                 "ویرایش",
//                 "weather",
//                 DateTime.UtcNow,
//                 new List<PropertyChange>
//                 {
//                     new(CurrentValue: 12,
//                         LastModifiedAt: DateTime.Now.AddDays(-12),
//                         ModifiedBy: "Ali Rezaei",
//                         PreviousValue: 0,
//                         PropertyName: "min degree")
//                 }
//             )
//         });
//     }
// }