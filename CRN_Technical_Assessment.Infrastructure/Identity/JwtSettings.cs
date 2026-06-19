using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Infrastructure.Identity
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings"; 
        public string Secret { get; set; } = string.Empty; 
        public string Issuer { get; set; } = string.Empty; 
        public string Audience { get; set; } = string.Empty; 
        public int ExpiryMinutes { get; set; }
        public int RefreshTokenExpiryDays { get; set; }
    }
}
