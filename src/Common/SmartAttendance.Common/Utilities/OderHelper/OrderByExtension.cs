namespace SmartAttendance.Common.Utilities.OderHelper;

public static class OrderByExtension
{
    public static IQueryable<T> OrderByDynamic<T>(
        this IQueryable<T> source,
        string             propertyName,
        bool               descending)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");
        var property  = Expression.PropertyOrField(parameter, propertyName);
        var selector  = Expression.Lambda(property, parameter);

        var method = descending ? "OrderByDescending" : "OrderBy";

        var result = typeof(Queryable).GetMethods()
            .First(m => m.Name == method && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type)
            .Invoke(null,
            [
                source,
                selector
            ]);

        return (IQueryable<T>)result!;
    }
}