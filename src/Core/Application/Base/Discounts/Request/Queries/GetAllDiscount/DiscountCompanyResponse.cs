namespace Shifty.Application.Base.Discounts.Request.Queries.GetAllDiscount;

public class DiscountCompanyResponse
{
    public string? CompanyName { get; set; }
    public DateTime? DateOfUse { get; set; } = null;
    public bool IsUsed { get; set; }
}