using CRN_Technical_Assessment.Domain.Common;
using CRN_Technical_Assessment.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Domain.Entities
{
    public class User:BaseEntities
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; }
            = UserRole.User;

        public UserStatus Status { get; set; }
            = UserStatus.Active;

        public Profile? Profile { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
            = new List<RefreshToken>();
    }
}
