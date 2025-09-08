namespace SmartAttendance.Common.Utilities.PaginationHelpers;

public class PagedResult<T> : PagedResultBase
    where T : class
{
    public IList<T> Results { get; set; } = new List<T>();

    public SortedDictionary<string, bool> Headers { get; set; } = null!;
}