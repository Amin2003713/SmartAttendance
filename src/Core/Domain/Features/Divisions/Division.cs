using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Shifts;
using Shifty.Domain.Users;
using System;
using System.Collections.Generic;

namespace Shifty.Domain.Divisions
{
    public class Division  : BaseEntity
    {
        public string Name  { get; set; }




        #region Relations

        public Shift? Shift { get; set; }
        public Guid? ShiftId { get; set; }


        // Self-referencing relationships
        public Guid? ParentId { get; set; }          // Foreign key for the parent
        public Division Parent { get; set; }         // Navigation property for the parent
        public List<Division> Children { get; set; } // Navigation property for the children

        public List<DivisionAssignee> Assignees { get; set; } = new();

        #endregion
    }


    public class DivisionAssignee : BaseEntity
    {
        public Guid DivisionId { get; set; }
        public Division Division { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public bool  IsManager { get; set; }
    }
}