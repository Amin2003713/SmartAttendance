namespace SmartAttendance.Domain.Common;

// استثنای دامنه عمومی
public class DomainException : Exception
{
    public DomainException(string message)
        : base(message) { }
}

// استثنای اعتبارسنجی

// استثنای قوانین کسب‌وکار

// استثنای دسترسی (RBAC)