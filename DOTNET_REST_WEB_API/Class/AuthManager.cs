using Dapper;
using DOTNET_REST_WEB_API.Helper;
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

        public TokenResponse GenerateJwt(UserModel model)
        {
            var res = new TokenResponse();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthManager:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim("id", model.UserId.ToString()),                      
            new Claim("roles", model.LoginId),
            new Claim("email", model.Email),
            new Claim(JwtRegisteredClaimNames.Sub, model.sub),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _config["AuthManager:Issuer"],
                _config["AuthManager:ValidAudience"],
                claims,
                expires: DateTime.UtcNow.AddSeconds(1800),
                signingCredentials: credentials
            );
            res.access_token = new JwtSecurityTokenHandler().WriteToken(token);
            res.expire_in = DateTime.Now.AddSeconds(1800).ToShortTimeString();
            res.token_type = "Bearer";
            return res;
        }

        public async Task<ServiceResponse<object>> RequestToken(string user, string pass)
        {
            var request = new ServiceResponse<object>();
            try
            {
                //connection.Close();
                if (connection.State == ConnectionState.Closed)
                {
                    //connection.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("user", user);
                    param.Add("type", "getuserlist");
                    param.Add("pass", pass);
                    param.Add("retval", DbType.Int32, direction: ParameterDirection.Output);
               
                    var res = await connection.QueryAsync<dynamic>("usercredAPI", param, commandType: CommandType.StoredProcedure);
                    int retval = param.Get<int>("retval");
                    if (retval.Equals(200))
                    {
                        UserModel model = new UserModel();
                        model.UserId = Convert.ToInt32(res.First().UserId.ToString());
                        model.Email = res.First().Email.ToString();

                        model.LoginId = res.First().LoginId.ToString();
                        model.UserName = res.First().UserName.ToString();
                        model.UserPass = res.First().UserPass.ToString();
                        model.sub = res.First().sub.ToString();

                        request.data = GenerateJwt(model);
                    }
                    else
                    {
                        request.data = new
                        {
                            StatusCode = 101,
                            Message = "No Record Fetch!"
                        };
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

        public async Task<ServiceResponse<object>> RequestTokenPlayer(string user, string pass)
        {
            var request = new ServiceResponse<object>();
            try
            {
                //connection.Close();
                if (connection.State == ConnectionState.Closed)
                {
                    //connection.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("user", user);
                    param.Add("type", "getinfolist1");
                    param.Add("pass", AES_Encryption.Encrypt(pass,user));
                    param.Add("retval", DbType.Int32, direction: ParameterDirection.Output);

                    var res = await connection.QueryAsync<dynamic>("usercredAPI", param, commandType: CommandType.StoredProcedure);
                    int retval = param.Get<int>("retval");
                    if (retval.Equals(200))
                    {
                        UserModel model = new UserModel();
                        model.UserId = Convert.ToInt32(res.First().UserID.ToString());
                        model.Email = res.First().UserName.ToString() + "@gmail.com";

                        model.LoginId = res.First().UserName.ToString();
                        model.UserName = res.First().UserName.ToString();
                        model.UserPass = res.First().Password.ToString();
                        model.sub = res.First().UserName.ToString();

                        request.data = GenerateJwt(model);
                    }
                    else
                    {
                        request.data = new
                        {
                            StatusCode = 101,
                            Message = "No Record Fetch!"
                        };
                    }
                }
            }
            catch (SqlException sql)
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
