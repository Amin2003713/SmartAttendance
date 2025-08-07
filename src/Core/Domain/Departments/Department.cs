using Shifty.Common.General.BaseClasses;

namespace Shifty.Domain.Departments;

public class Department : BaseEntity
{
    public string Title { set; get; }

    public Guid? ParentDepartmentId { set; get; }

    public Department? ParentDepartment { set; get; }

    public Guid? ManagerId { set; get; }

    public User? Manager { set; get; }

    public virtual ICollection<User>? Users { get; set; }
    public virtual ICollection<Department>? Children { get; set; }
}