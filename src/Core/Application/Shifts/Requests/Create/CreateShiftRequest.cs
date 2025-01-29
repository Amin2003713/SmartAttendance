using System;

namespace Shifty.Application.Shifts.Requests.Create;

public class CreateShiftRequest
{
    /// <summary>
    /// Name of the shift.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Start time of the shift.
    /// </summary>
    public TimeOnly Arrive { get; set; }

    /// <summary>
    /// End time of the shift.
    /// </summary>
    public TimeOnly Leave { get; set; }

    /// <summary>
    /// Grace period allowed for late arrival.
    /// </summary>
    public TimeSpan GraceLateArrival { get; set; }

    /// <summary>
    /// Grace period allowed for early leave.
    /// </summary>
    public TimeSpan GraceEarlyLeave { get; set; }
}