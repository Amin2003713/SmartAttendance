namespace Shifty.Common.General.Enums.Projects;

public enum UserType : byte
{
    [Display(Name = "هیچ")]        None           = 0,
    [Display(Name = "پیمانکار")]   Contractor     = 1,
    [Display(Name = "ناظر سایت")]  SiteSupervisor = 2,
    [Display(Name = "مدیر پروژه")] ProjectManager = 3,
    [Display(Name = "ذینفع")]      Stakeholder    = 4
}