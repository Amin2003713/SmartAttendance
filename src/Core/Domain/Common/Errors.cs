namespace SmartAttendance.Domain.Common;

// استثنای دامنه عمومی
public class DomainException : Exception
{
	public DomainException(string message) : base(message) { }
}

// استثنای اعتبارسنجی
public class DomainValidationException : DomainException
{
	public DomainValidationException(string message) : base(message) { }
}

// استثنای قوانین کسب‌وکار
public class BusinessRuleViolationException : DomainException
{
	public BusinessRuleViolationException(string message) : base(message) { }
}

// استثنای دسترسی (RBAC)
public class ForbiddenOperationException : DomainException
{
	public ForbiddenOperationException(string message) : base(message) { }
}

