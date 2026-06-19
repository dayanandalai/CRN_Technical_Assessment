using CRN_Technical_Assessment.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Infrastructure.Identity
{
    public class PasswordHasher : IPasswordHasher 
    { 
        public string HashPassword(string password) 
        { 
            return BCrypt.Net.BCrypt.HashPassword(password); 
        }
        public bool VerifyPassword(string password, string passwordHash) 
        { 
            return BCrypt.Net.BCrypt.Verify(password, passwordHash); 
        } 
    }
}
