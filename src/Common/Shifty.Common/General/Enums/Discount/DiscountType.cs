namespace Shifty.Common.General.Enums.Discount;

public enum DiscountType : byte
{
    [Display(Name = "درصدی")]     Percent,
    [Display(Name = "تومانی")]    FixedAmount,
    [Display(Name = "روز اضافی")] ExtraDays
}