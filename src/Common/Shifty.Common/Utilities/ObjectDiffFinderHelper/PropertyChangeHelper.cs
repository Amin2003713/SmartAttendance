using Shifty.Common.Common.ESBase;
using Shifty.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using Shifty.Common.Common.Responses.PropertyChanges;

namespace Shifty.Common.Utilities.ObjectDiffFinderHelper;

public static class PropertyChangeHelper
{
    public static List<PropertyChange> GetDifferencesFromEvent(
        string typeName,
        string oldDataJson,
        string newDataJson,
        DateTime lastModifiedAt,
        string modifiedBy,
        Func<string, Type?>? typeResolver = null)
    {
        var diffs = new List<PropertyChange>();

        // مشخص کردن نوع بر اساس نام
        var type = typeResolver?.Invoke(typeName) ?? FindTypeInAllAssemblies(typeName);
        var baseType = typeof(IDomainEvent).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(a => a.Name)
            .ToList();

        if (type == null)
            return diffs;

        var oldObj = JsonSerializer.Deserialize(oldDataJson, type, ApplicationConstant.Mongo.JsonOptions);
        var newObj = JsonSerializer.Deserialize(newDataJson, type, ApplicationConstant.Mongo.JsonOptions);

        if (oldObj == null || newObj == null)
            return diffs;

        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        diffs.AddRange(
            from prop in props
            where !baseType.Contains(prop.Name)
            let oldValue = prop.GetValue(oldObj)?.ToString()?.Trim()
            let newValue = prop.GetValue(newObj)?.ToString()?.Trim()
            where oldValue != newValue && (oldValue != null || newValue != null)
            select new PropertyChange(prop.Name,
                oldValue,
                newValue,
                lastModifiedAt,
                modifiedBy));

        return diffs;
    }

    private static Type? FindTypeInAllAssemblies(string classNameOnly)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                // Match class name (not namespace-qualified)
                var match = assembly.GetTypes().FirstOrDefault(t => t.Name == classNameOnly);

                if (match != null)
                    return match;
            }
            catch (ReflectionTypeLoadException)
            {
                // Skip any problematic assemblies (like dynamic or reflection-only)
            }
        }

        return null;
    }


    // public static SortedDictionary<DateTime, GetResentActivities> ToRecentActivities<TId>(
    //     this AggregateRoot<TId> aggregate,
    //     string itemPersianName,
    //     Dictionary<Guid, LogPropertyInfoResponse> users)
    // {
    //     var result = new SortedDictionary<DateTime, GetResentActivities>();
    //
    //     var sortedEvents = aggregate.StoredEvents.OrderBy(e => e.Version);
    //
    //     EventDocument<TId>? previousEvent = null;
    //
    //     foreach (var currentEvent in sortedEvents)
    //     {
    //         var (label, requiresComparison) = currentEvent.Type switch
    //                                           {
    //                                               var t when t.Contains("Created")                 => ("درج آیتم", false),
    //                                               var t when t.Contains("CommentAdded")            => ("درج نظر", false),
    //                                               var t when t.Contains("Updated")                 => ("ویرایش", true),
    //                                               var t when t.Contains("Deleted")                 => ("حذف", false),
    //                                               var t when t.Contains("CommentDeletedFromAdded") => ("حذف نظر", false),
    //                                               var t when t.Contains("Rejected")                => ("رد", false),
    //                                               var t when t.Contains("Verified")                => ("تایید", false),
    //                                               _                                                => ("تغییر", false)
    //                                           };
    //
    //         var changes = requiresComparison && previousEvent != null
    //             ? GetDifferencesFromEvent(
    //                 currentEvent.Type,
    //                 previousEvent.Data,
    //                 currentEvent.Data,
    //                 currentEvent.OccurredOn,
    //                 currentEvent.UserId.ToString()
    //             )
    //             : new List<PropertyChange>();
    //
    //         // Try to get user's name
    //         var fullName = users.TryGetValue(currentEvent.UserId, out var userInfo)
    //             ? userInfo.FullName
    //             : "نامشخص";
    //
    //         result[currentEvent.OccurredOn] = new GetResentActivities(
    //             "", // If needed, pass projectId as parameter
    //             fullName,
    //             label,
    //             itemPersianName,
    //             currentEvent.OccurredOn,
    //             changes
    //         );
    //
    //         if (currentEvent.Type.Contains("Created") || currentEvent.Type.Contains("Updated"))
    //             previousEvent = currentEvent;
    //     }
    //
    //     return result;
    // }
}