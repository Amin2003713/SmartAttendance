using System.Collections.Generic;
using Mapster;
using Shifty.Domain.Defaults;
using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Shifts.Queries.GetDefault;

public class ListShiftsQueryResponseExamples : IExamplesProvider<List<ListShiftsQueryResponse>>
{
    public List<ListShiftsQueryResponse> GetExamples()
        => Defaults.GetDefaultShifts().Adapt<List<ListShiftsQueryResponse>>();
}