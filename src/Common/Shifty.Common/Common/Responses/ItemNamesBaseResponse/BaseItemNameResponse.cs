namespace Shifty.Common.Common.Responses.ItemNamesBaseResponse;

public class BaseItemNameResponse
{
    public Guid Id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? ProjectId { get; set; } = null!;

    public string Name { get; set; }
}