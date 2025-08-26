
using Shifty.Common.General.Enums.Workflows;

namespace Shifty.Application.Base.Calendars.Request.Queries.GetCalendarLevelReport;

public class CalendarInfoForLevelDto
{
    public string ItemNamePersian { get; set; }
    public string ItemName { get; set; }
    public ItemTypes Type { get; set; }
}