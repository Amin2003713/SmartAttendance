namespace SmartAttendance.Domain.Common;

public class BusinessRuleViolationException : DomainException
{
    public BusinessRuleViolationException(string message)
        : base(message) { }
}