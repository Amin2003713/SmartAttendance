namespace Shifty.Common.General.Enums;

public enum Roles : byte
{
    [Display(Name = "پشتیبانی")]   Support,
    [Display(Name = "مدیر سیستم")] Admin,

    [Display(Name = "مدیریت کاربران (مدیر سیستم)")]
    ManageUsers,

    [Display(Name = "مدیریت پروژه‌ها (مدیر سیستم)")]
    ManageProjects,

    [Display(Name = "مدیریت پیام‌ها (مدیر سیستم)")]
    ManageMessages,

    [Display(Name = "ایجاد کاربر")]  Users_Create,
    [Display(Name = "ویرایش کاربر")] Users_Edit,
    [Display(Name = "حذف کاربر")]    Users_Delete,
    [Display(Name = "ایجاد پروژه")]  Projects_Create,
    [Display(Name = "ویرایش پروژه")] Projects_Edit,
    [Display(Name = "حذف پروژه")]    Projects_Delete,
    [Display(Name = "آرشیو پروژه")]  Projects_Archive,
    [Display(Name = "ایجاد پیام")]   Messages_Create,
    [Display(Name = "ویرایش پیام")]  Messages_Edit,
    [Display(Name = "حذف پیام")]     Messages_Delete,
    [Display(Name = "مشاهده پیام")]  Messages_Read
}