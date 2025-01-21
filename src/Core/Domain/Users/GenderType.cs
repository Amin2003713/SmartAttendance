using System.ComponentModel.DataAnnotations;

namespace Shifty.Domain.Users
{
    public enum GenderType
    {
        [Display(Name = "مرد")]          Male     = 1 ,
        [Display(Name = "زن")]           Female   = 2 ,
        [Display(Name = "انتخواب نشده")] UnDefine = 3 ,
    }
}