namespace SmartAttendance.Domain.Common;

public class ForbiddenOperationException : DomainException
{
    public ForbiddenOperationException(string message)
        : base(message) { }
}