namespace Shifty.Common.BaseReportingResponse;

public class BaseReportingResponse<TData>
{
    public List<TData> Data { get; set; } = null!;
    public SortedDictionary<string, bool> Headers { get; set; } = null!;
}