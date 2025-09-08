namespace SmartAttendance.Common.General.Enums;

public enum TasksStatus : byte
{
    [Display(Name = "برنامه ریزی شده")] Planned,
    [Display(Name = "در حال انجام ")]   InProgress,
    [Display(Name = "تکمیل شده")]       LCompleted,
    [Display(Name = "با تاخیر")]        Delayed
}