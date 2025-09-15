using System.Collections.Concurrent;
using Microsoft.Extensions.Localization;

namespace SmartAttendance.Common.Utilities.DynamicTableHelper;

public class TableTranslatorService<THandler>(
    IStringLocalizer<THandler> localizer
) : ITableTranslatorService<THandler>
{
    // Cache property info for each type to avoid repeated reflection.
    private readonly ConcurrentDictionary<Type, List<PropertyInfo>> _propertyCache = new ConcurrentDictionary<Type, List<PropertyInfo>>();

    // Retrieves all column (property) names for type T.
    public List<string> GetColumnNames<T>()
    {
        return GetProperties<T>().Select(prop => localizer[prop.Name].Value).ToList();
    }

    // Retrieves property names for type T filtered by the provided predicate.
    public List<string> GetColumnNames<T>(Func<PropertyInfo, bool> predicate)
    {
        return GetProperties<T>().Where(predicate).Select(prop => localizer[prop.Name].Value).ToList();
    }

    /// <summary>
    ///     Returns a sorted dictionary where each key is the column (property) name and
    ///     the value is true if the column is summable (numeric), otherwise false.
    ///     The dictionary is sorted by the column names.
    /// </summary>
    public SortedDictionary<string, bool> GetColumnInfos<T>()
    {
        var sortedColumns = new SortedDictionary<string, bool>();

        foreach (var prop in GetProperties<T>())
        {
            sortedColumns[localizer[prop.Name].Value] = IsNumericType(prop.PropertyType);
        }

        return sortedColumns;
    }

    // Checks if a particular column (by name) in type T is summable (numeric).
    public bool CanSumColumn<T>(string columnName)
    {
        // Try to locate the property on type T.
        var prop = typeof(T).GetProperty(columnName, BindingFlags.Public | BindingFlags.Instance);

        if (prop == null)
            throw new ArgumentException($"Column '{columnName}' does not exist in type '{typeof(T).Name}'.");

        return IsNumericType(prop.PropertyType);
    }

    // Retrieves all public instance properties for a given type.
    private List<PropertyInfo> GetProperties<T>()
    {
        return _propertyCache.GetOrAdd(typeof(T),
                                       type => type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList());
    }

    // Determines if a type is numeric (supports nullable numeric types as well).
    private static bool IsNumericType(Type type)
    {
        // Unwrap nullable types
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        return Type.GetTypeCode(underlyingType) switch
               {
                   TypeCode.Byte or TypeCode.SByte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64 or TypeCode.Int16
                       or TypeCode.Int32 or TypeCode.Int64
                       or TypeCode.Decimal or TypeCode.Double or TypeCode.Single => true,
                   _ => false
               };
    }
}