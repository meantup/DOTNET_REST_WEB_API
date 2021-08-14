using DOTNET_REST_WEB_API.Model;
using DOTNET_REST_WEB_API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Helper
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        public JwtMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }
        public async Task Invoke(HttpContext context, IAuthManager userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //context.Items
            if(token != null)
                attachUserContext(context, userService, token);
                
            await _next(context);                                           
        }
        public void attachUserContext(HttpContext context, IAuthManager userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthManager:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                },out SecurityToken validateToken);
                var jwtToken = (JwtSecurityToken)validateToken;
                UserInfo users = new UserInfo();
                users.UserId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                users.LoginId = jwtToken.Claims.First(x => x.Type == "roles").Value;
                users.Email = jwtToken.Claims.First(x => x.Type == "email").Value;
                users.sub = jwtToken.Claims.First(x => x.Type == "sub").Value;

                //attach user to context on successful jwt validation
                context.Items["User"] = users;
            }
            catch (Exception ee)
            {
                return;
            }
        }
    }
}
