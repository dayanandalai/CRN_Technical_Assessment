using CRN_Technical_Assessment.Application.Interfaces;
using CRN_Technical_Assessment.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Infrastructure.Identity
{
    public class JwtTokenGenerator : IJwtTokenGenerator 
    { 
        private readonly JwtSettings _settings;
        public JwtTokenGenerator(IOptions<JwtSettings> settings) 
        {
            _settings = settings.Value ?? throw new InvalidOperationException("JwtSettings is not configured");
            if (string.IsNullOrEmpty(_settings.Secret))
                throw new InvalidOperationException("JwtSettings.Secret is not configured. Please set a valid secret key in appsettings.json");
        } 
        public string GenerateToken(User user) 
        { var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.Secret));
            var credentials = new SigningCredentials(
                key, 
                SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim> 
            { 
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), 
                new(JwtRegisteredClaimNames.Email, user.Email), 
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role.ToString()) 
            }; 
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims, 
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes), 
               signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        { 
            var randomBytes = new byte[64]; 
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes); 
            return Convert.ToBase64String(randomBytes); 
        } 
    }
}
