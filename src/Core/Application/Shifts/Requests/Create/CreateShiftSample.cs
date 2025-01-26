using Swashbuckle.AspNetCore.Filters;
using System;

namespace Shifty.Application.Shifts.Requests.Create;

public class CreateShiftSample : IExamplesProvider<CreateShiftRequest>
{
    /// <summary>
    /// Provides example data for creating a shift.
    /// </summary>
    /// <returns>An example of <see cref="CreateShiftRequest"/>.</returns>
    public CreateShiftRequest GetExamples()
    {
        return new CreateShiftRequest
        {
            Name             = "Morning Shift",
            Arrive            = new TimeOnly(9,  0),      // 9:00 AM
            Leave              = new TimeOnly(17, 0),      // 5:00 PM
            GraceLateArrival = TimeSpan.FromMinutes(10), // 10-minute grace
            GraceEarlyLeave  = TimeSpan.FromMinutes(25)   // 5-minute grace
        };
    }
}