using DOTNET_REST_WEB_API.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Class
{
    public class RefreshToken : IRefreshToken
    {
        private readonly IConfiguration config;
        public RefreshToken(IConfiguration _config)
        {
            config = _config;
        }
        public string GenerateToken(string secretkey,string issuer,string audience,double expirationMinutes)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AuthManager:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
             issuer,
             audience,
             claims: null,
             notBefore: DateTime.UtcNow,
             expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
             signingCredentials: credentials
             );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
