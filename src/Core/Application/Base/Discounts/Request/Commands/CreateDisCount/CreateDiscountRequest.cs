using SmartAttendance.Common.General.Enums.Discount;

namespace SmartAttendance.Application.Base.Discounts.Request.Commands.CreateDisCount;

public class CreateDiscountRequest
{
    public string Code { get; set; }
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }

    public DiscountType DiscountType { get; set; }

    public decimal Value { get; set; }

    public int? PackageMonth { get; set; }

    public List<string> TenantIds { get; set; } = [];
}