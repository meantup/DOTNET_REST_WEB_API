using Dapper;
using DOTNET_REST_WEB_API.Model;
using DOTNET_REST_WEB_API.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Class
{
    public class AuthManager : IAuthManager
    {
        private readonly IConfiguration _config;
        private SqlConnection connection;
       
        public AuthManager(IConfiguration config)
        {
            _config = config;
            connection = new SqlConnection(_config["ConnectionStrings:AccountConnection"]);
        }

        public TokenResponse<object> GenerateJwt(UserModel model)
        {
            var tokenRes = new TokenResponse<object>();
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthManager:Key"]));
                var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenExpire = DateTime.UtcNow.AddSeconds(1800).Second;
                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", model.UserId.ToString()), new Claim("roles", model.LoginId), new Claim("email", model.Email), new Claim("sub", model.sub), new Claim(JwtRegisteredClaimNames.Sub, model.sub), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) }),
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(5),  
                    SigningCredentials = credentials
                };
                var token = tokenHandler.CreateToken(tokenDescription);
                tokenRes.token = tokenHandler.WriteToken(token);
                tokenRes.token_Type = "Bearer";
                tokenRes.token_Expired = DateTime.UtcNow.AddSeconds(1800).Second;
            }
            catch (SqlException sql)
            {
                tokenRes.token_Type = new
                {
                    message = sql.Message,
                    error = sql.Errors,
                    resCode = -100
                };
            }
            catch(Exception ee)
            {
                tokenRes.token_Type = new
                {
                    message = ee.Message,
                    resCode = 500
                };
            }
            return tokenRes;
        }

        public async Task<ServiceResponse<object>> RequestToken(string user, string pass)
        {
            ConnectionState state = connection.State;
            var request = new ServiceResponse<object>();
            try
            {
                if (state == ConnectionState.Closed)
                {
                    connection.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("user", user);
                    param.Add("type", "getuserlist");
                    param.Add("pass", pass);
                    param.Add("retval", DbType.Int32, direction: ParameterDirection.Output);
               
                    var res = await connection.QueryAsync<UserModel>("usp_usercredAPI", param, commandType: CommandType.StoredProcedure);
                    int retval = param.Get<int>("retval");
                    if (retval.Equals(200))
                    {
                        var ret = res.ToList();
                        UserModel model = new UserModel();
                        model.UserId = Convert.ToInt32(ret[0].UserId.ToString());
                        model.Email = ret[0].Email.ToString();
                        model.LoginId = ret[0].LoginId.ToString();
                        model.UserName = ret[0].UserName.ToString();
                        model.UserPass = ret[0].UserPass.ToString();
                        model.sub = ret[0].sub.ToString();
                        request.data = GenerateJwt(model);
                    }
                }
            }
            catch(SqlException sql)
            {
                request.data = new
                {
                    StatusCode = sql.ErrorCode,
                    Message = "SqlException Error!",
                    Error = sql.Message
                };
            }
            catch (Exception ee)
            {
                request.data = new
                {
                    StatusCode = 500,
                    Message = "Exception Error!",
                    Error = ee.Message
                };
            }
            return request;
        }

    }
}
