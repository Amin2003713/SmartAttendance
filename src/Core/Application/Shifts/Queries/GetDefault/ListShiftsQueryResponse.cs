using System;

namespace Shifty.Application.Shifts.Queries.GetDefault
{
    public class ListShiftsQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TimeOnly Arrive { get; set; }
        public TimeOnly Leave { get; set; }
        public TimeSpan GraceLateArrival { get; set; }
        public TimeSpan GraceEarlyLeave { get; set; }
    }
}