using Dapper;
using DOTNET_REST_WEB_API.Helper;
using DOTNET_REST_WEB_API.Model;
using DOTNET_REST_WEB_API.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Class
{
    public class DefaultClass : IDefault
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostingEnvironment;
        private SqlConnection sql;
        public DefaultClass(IConfiguration _config, IWebHostEnvironment _hostingEnvironment)
        {
            config = _config;
            hostingEnvironment = _hostingEnvironment;
            sql = new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
        }

     
        public async Task<ServiceResponseT<object>> insert(InsertProduct insert)
        {
            var response = new ServiceResponseT<object>();
            try
            {
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("retval", DbType.Int32, direction: ParameterDirection.Output);
                var property = insert.GetType().GetProperties();
                foreach (var item in property)
                {
                    var name = item.Name;
                    var value = item.GetValue(insert);
                    dynamic.Add(name, value);
                }
                var request = await sql.ExecuteAsync("usp_InsertItemOrder",dynamic,commandType: CommandType.StoredProcedure);
                int retval = dynamic.Get<int>("retval");
                if (retval.Equals(100))
                {
                    response.ResponseCode = 200;
                    response.Message = "Successfully Inserted!";
                }
                else
                {
                    response.ResponseCode = 200;
                    response.Message = "Unsuccessful";
                }
            }
            catch (SqlException sqlex)
            {
                response.ResponseCode = 501;
                response.Message = "SqlException Error Type";
                response.Data = sqlex.Message;
            }
            catch (Exception ee)
            {
                response.ResponseCode = 500;
                response.Message = "Exception Error Type";
                response.Data = ee.Message;
            }
            return response;
        }
        public async Task<ServiceResponseT<List<OrderList>>> inquiryList(string from, string to)
        {
            var response = new ServiceResponseT<List<OrderList>>();
            try
            {
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("retval", DbType.Int32, direction: ParameterDirection.Output);
                dynamic.Add("from", from);
                dynamic.Add("to", to);
                var ss = sql.Query<OrderList>("usp_InquiryDate", dynamic, commandType: CommandType.StoredProcedure).AsList();
                int retval = dynamic.Get<int>("retval");
                if (retval.Equals(100))
                {
                    response.ResponseCode = 200;
                    response.Message = "Has Record Found!";
                    response.Data = ss;
                }
                else
                {
                    response.ResponseCode = 101;
                    response.Message = "No Record Found!";
                    response.Data = ss;
                }
            }
            catch (SqlException sqlex)
            {
                response.ResponseCode = 501;
                response.Message = "SqlException Error Type";
            }
            catch (Exception ee)
            {
                response.ResponseCode = 500;
                response.Message = "Exception Error Type";
            }
            return response;
        }



        public async Task<string> path(IFormFile image)
        {
            if (CheckIfImageFile(image))
            {
                var res = await WriteFile(image);
                return res;
            }
            return "Invalid Image File";
        }

     
        public async Task<string> WriteFile(IFormFile file)
        {
            string fileName;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                //var fileName = Path.GetFileName(image.FileName);
                fileName = Guid.NewGuid().ToString() + extension; //Create a new Name for the file due to security reasons.
                var filePath = Path.Combine(hostingEnvironment.WebRootPath, "Images\\", fileName);

                using (var bits = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return fileName;
        }

        private bool CheckIfImageFile(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
            return ImageWriter.GetImageFormat(fileBytes) != ImageWriter.ImageFormat.unknown;
        }
    }
}
