using DOTNET_FRONT_END.Models;
using DOTNET_REST_WEB_API.Model;
using DOTNET_REST_WEB_API.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DOTNET_REST_WEB_API.Helper;
using System.Data;

namespace DOTNET_REST_WEB_API.Class
{
    public class AccountClass : IAccount
    {
        private readonly IConfiguration _config;
        private SqlConnection connection;

        public AccountClass(IConfiguration config)
        {
            _config = config;
            connection = new SqlConnection(_config["ConnectionStrings:AccountConnection"]);
        }

        public async Task<ServiceResponseT<object>> getUserList()
        {
            var service = new ServiceResponseT<object>();
            string type = "getinfolist";

            try
            {
                DynamicParameters params1 = new DynamicParameters();
                params1.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);
                params1.Add("@type", type);
                var Result = await connection.QueryAsync<dynamic>("usercredAPI", params1, commandType: CommandType.StoredProcedure);
                var retval = params1.Get<int>("retval");

                service.ResponseCode = 200;
                if (retval != 200)
                {
                    service.Message = "Succesfully Get Data";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Result = Result
                    };
                }
                else
                {
                    service.Message = "No Data Found";
                    service.Data = new
                    {
                        ResponseCode = retval,
                    };
                }

            }
            catch (Exception ee)
            {
                service.ResponseCode = 500;
                service.Message = "Exception";
                service.Data = new
                {
                    ResponseCode = 99,
                    Result = new
                    {
                        ErrorMessage = ee.Message
                    }
                };
            }
            return service;
        }
    

        public async Task<ServiceResponseT<object>> signUp(SignUp creds)
        {
            var service = new ServiceResponseT<object>();
            
            string type = "Signup";
            try
            {
                creds.password = AES_Encryption.Encrypt(creds.password,creds.username);
                var params1 = PropertyType.parameters(creds);
                params1.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);
                params1.Add("@type", type);
                var Result = await connection.QueryAsync<dynamic>("usercredAPI", params1, commandType: CommandType.StoredProcedure);
                var retval = params1.Get<int>("retval");

                service.ResponseCode = 200;
                if (retval == 100)
                {
                    service.ResponseCode = 100;
                    service.Message = "Sucessfully insert";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Result = Result
                    };
                }
                else if(retval == 404)
                {
                    service.ResponseCode = 404;
                    service.Message = "Username already used.";
                    service.Data = new
                    {
                        ResponseCode = retval,
                    };
                }
                else
                {
                    service.ResponseCode = retval;
                    service.Message = "Not Inserted";
                    service.Data = new
                    {
                        ResponseCode = retval,
                    };
                }

            }
            catch (Exception ee)
            {
                service.ResponseCode = 500;
                service.Message = "Exception";
                service.Data = new
                {
                    ResponseCode = 99,
                    Result = new
                    {
                        ErrorMessage = ee.Message
                    }
                };
            }
            return service;
        }
    }
}
