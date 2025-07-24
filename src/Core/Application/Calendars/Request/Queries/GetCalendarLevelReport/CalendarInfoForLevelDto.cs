using Shifty.Common.General.Enums.Projects;
using Shifty.Common.General.Enums.Workflows;

namespace Shifty.Application.Calendars.Request.Queries.GetCalendarLevelReport;

public class CalendarInfoForLevelDto
{
    public OrderedDictionary<UserType, string> Data { get; set; }
    public string ItemNamePersian { get; set; }
    public string ItemName { get; set; }
    public ItemTypes Type { get; set; }
}