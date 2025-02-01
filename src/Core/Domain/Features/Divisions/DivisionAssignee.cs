using System;
using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Features.Users;

namespace Shifty.Domain.Features.Divisions;

public class DivisionAssignee : BaseEntity
{
    public Guid DivisionId { get; set; }
    public Division Division { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public bool  IsManager { get; set; }
}