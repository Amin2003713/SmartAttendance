using System.ComponentModel.DataAnnotations;

namespace App.Common.General.Enums.FileType;

public enum FileType
{
    [Display(Name = "تصویر")]      Picture,
    [Display(Name = "فایل PDF")]   Pdf,
    [Display(Name = "فایل ZIP")]   Zip,
    [Display(Name = "فایل Excel")] Excel,
    [Display(Name = "فایل Xml")]   Xml
}