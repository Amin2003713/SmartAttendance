using System;
using Microsoft.AspNetCore.Identity;
using Shifty.Domain.Common.BaseClasses;

namespace Shifty.Domain.Features.Users
{
    public class Role : IdentityRole<Guid> , ISimpleEntity
    {
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}