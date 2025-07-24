using Shifty.Common.InjectionHelpers;

namespace Shifty.Common.Utilities.DynamicTableHelper;

public interface ITableTranslatorService<THandler> : ISingletonDependency
{
    List<string>                   GetColumnNames<T>();
    List<string>                   GetColumnNames<T>(Func<PropertyInfo, bool> predicate);
    SortedDictionary<string, bool> GetColumnInfos<T>();
    bool                           CanSumColumn<T>(string columnName);
}