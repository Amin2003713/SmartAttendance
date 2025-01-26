using MediatR;
using System;

public class CreateShiftCommand : IRequest<CreateShiftResponse>
{
    /// <summary>
    /// Name of the shift.
    /// </summary>
    public string Name { get; set; }

    public TimeOnly Arrive { get; set; }


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