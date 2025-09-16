namespace SmartAttendance.Domain.Common;

public class DomainValidationException : DomainException
{
    public DomainValidationException(string message)
        : base(message) { }
}