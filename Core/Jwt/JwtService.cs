using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Jw;
using Data.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Core.Jwt
{
    public class JwtService
    {
        private readonly JwtOptions _options;
        public JwtService(IOptions<JwtOptions> options) 
        {
                _options = options.Value;
        }

        public string GenerateTBearerToken(User user)
        {   
            var key = new SymmetricSecurityKey(Convert.FromBase64String(_options.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var claims = new List<Claim>() 
            {
                new Claim(ClaimTypes.Name,user.Username)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                subject: new ClaimsIdentity(claims),
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials 
            );

            return handler.WriteToken(token);
        }
    }
}