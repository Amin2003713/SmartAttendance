using System;
using Shifty.Domain.Common.BaseClasses;

namespace Shifty.Domain.Features.Users
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}