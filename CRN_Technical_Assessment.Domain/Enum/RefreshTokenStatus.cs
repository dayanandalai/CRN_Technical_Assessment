using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Domain.Enum
{
    public enum RefreshTokenStatus
    {
        Active = 1,
        Revoked = 2,
        Expired = 3
    }
}
