using System;
using MediatR;

namespace Shifty.Application.Shifts.Command.Create;

public class CreateShiftCommand : IRequest
{
    /// <summary>
    /// Name of the shift.
    /// </summary>
    public required string Name { get; set; }

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