using CRN_Technical_Assessment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user); 
        string GenerateRefreshToken();
    }
}
