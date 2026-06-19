using CRN_Technical_Assessment.Domain.Common;
using CRN_Technical_Assessment.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Domain.Entities
{
    public class RefreshToken:BaseEntities
    {
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public RefreshTokenStatus Status { get; set; }
            = RefreshTokenStatus.Active;

        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
