using System;

public class CreateShiftResponse
{
    /// <summary>
    /// Identifier of the created shift.
    /// </summary>
    public Guid ShiftId { get; set; }

    /// <summary>
    /// Confirmation message.
    /// </summary>
    public string Message { get; set; }
}