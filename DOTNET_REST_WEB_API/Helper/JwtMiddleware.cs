using DOTNET_REST_WEB_API.Model;
using DOTNET_REST_WEB_API.Repository;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
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
        private readonly IAuthManager _userService;

        private IJsonSerializer _serializer = new JsonNetSerializer();
        private IDateTimeProvider _provider = new UtcDateTimeProvider();
        private IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
        private IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();

        public JwtMiddleware(RequestDelegate next, IConfiguration config, IAuthManager userService)
        {
            _next = next;
            _config = config;
            _userService = userService;
        }
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            context.Items["expired_token"] = false;
            if (token != null)
                attachUserContext(context, token);
                
            await _next(context);                                           
        }
        public void attachUserContext(HttpContext context, string token)
        {
            try
            {
                IJwtValidator _validator = new JwtValidator(_serializer, _provider);
                IJwtDecoder decoder = new JwtDecoder(_serializer, _validator, _urlEncoder, _algorithm);
                var tokenExpr = decoder.DecodeToObject<JwtToken>(token);
                DateTimeOffset dtOffset = DateTimeOffset.FromUnixTimeSeconds(tokenExpr.exp);
                var tokenExpired = dtOffset.LocalDateTime;

                if (DateTime.Now > tokenExpired)
                {
                    context.Items["expired_token"] = true;
                }
                else
                {
                    context.Items["expired_token"] = false;
                    var tokenHandler = new JwtSecurityTokenHandler();
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthManager:Key"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero,
                    }, out SecurityToken validateToken);
                    var jwtToken = (JwtSecurityToken)validateToken;
                    UserInfo users = new UserInfo();
                    users.UserId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                    users.LoginId = jwtToken.Claims.First(x => x.Type == "roles").Value;
                    users.Email = jwtToken.Claims.First(x => x.Type == "email").Value;
                    users.sub = jwtToken.Claims.First(x => x.Type == "sub").Value;

                    //attach user to context on successful jwt validation
                    context.Items["User"] = users;
                }               
            }
            catch (Exception ee)
            {
                return;
            }
        }
    }
}
