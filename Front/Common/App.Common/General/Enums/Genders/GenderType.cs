using System.ComponentModel.DataAnnotations;

namespace App.Common.General.Enums.Genders;

public enum GenderType
{
    [Display(Name = "مرد")] Man,
    [Display(Name = "زن")]  Woman
}