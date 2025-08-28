using System.ComponentModel.DataAnnotations;

namespace Shifty.Common.General.Enums.StationStatuses
{
    public enum StationStatus
    {
        [Display(Name = "کامل")] Complete,

        [Display(Name = "ناقص")] Incomplete,

        [Display(Name = "هیجکدام")] None,

        [Display(Name = "خارج از محدوده")] OutOfRange
    }
}