using Microsoft.AspNetCore.Identity;
using Shifty.Domain.Common.BaseClasses;
using System;

namespace Shifty.Domain.Users
{
    public class Role : IdentityRole<Guid> , ISimpleEntity
    {
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}