using System;
using System.Collections.Generic;
using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Features.Divisions;

namespace Shifty.Domain.Features.Shifts
{
    public class Shift  : BaseEntity
    {
        public string Name { get; set; }

        public TimeOnly Arrive { get; set; }
        public TimeOnly Leave { get; set; }
        public TimeSpan GraceLateArrival { get; set; }
        public TimeSpan GraceEarlyLeave { get; set; }




        public List<Division>? Divisions { get; set; }
    }
}