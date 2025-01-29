using System;
using System.Collections.Generic;
using Mapster;
using Shifty.Domain.Defaults;
using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Shifts.Queries.GetDefault
{
    public class ListShiftsQueryResponse
    {
        public string Name { get; set; }

        public TimeOnly Arrive { get; set; }
        public TimeOnly Leave { get; set; }
        public TimeSpan GraceLateArrival { get; set; }
        public TimeSpan GraceEarlyLeave { get; set; }
    }


    public class ListShiftsQueryResponseExamples : IExamplesProvider<List<ListShiftsQueryResponse>>
    {
        public List<ListShiftsQueryResponse> GetExamples()
            => Defaults.GetDefaultShifts().Adapt<List<ListShiftsQueryResponse>>();
    }
}