using DNTPersianUtils.Core;
using Shifty.Common.General.Enums.Discount;

namespace Shifty.Application.Discounts.Request.Queries.GetAllDiscount;

public class GetAllDiscountResponse
{
    public DateTime CreatedAt { get; set; }
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Duration { get; set; }
    public bool IsActive { get; set; }
    public TimeSpanParts RemainingDays { get; set; }
    public List<DiscountCompanyResponse> DiscountCompanyUsage { get; set; }
    public DiscountType DiscountType { get; set; }

    public decimal Value { get; set; }
    public int? PackageMonth { get; set; }
}