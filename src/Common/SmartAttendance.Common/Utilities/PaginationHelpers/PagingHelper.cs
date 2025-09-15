namespace SmartAttendance.Common.Utilities.PaginationHelpers;

// ReSharper disable once PossibleMultipleEnumeration
public static class PagingHelper
{
    public static List<T> GetPaged<T>(
        this OrderedParallelQuery<T> query,
        int                          page,
        int                          pageSize,
        out int                      rowCount,
        out int                      pageCount)
        where T : class
    {
        if (query == null || !query.Any())
        {
            rowCount  = 0;
            pageCount = 0;
            return [];
        }


        rowCount = query.Count();

        pageSize = pageSize == int.MaxValue ? rowCount : pageSize;

        pageCount = (int)Math.Ceiling((double)rowCount / pageSize);

        var skip = (page - 1) * pageSize;
        return query.Skip(skip).Take(pageSize).ToList();
    }
}