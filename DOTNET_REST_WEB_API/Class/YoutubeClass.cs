using DOTNET_REST_WEB_API.Model;
using DOTNET_REST_WEB_API.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.IO;
using static DOTNET_REST_WEB_API.Model.YoutubeModel;
using DOTNET_REST_WEB_API.Helper;
using Dapper;
using System.Data;

namespace DOTNET_REST_WEB_API.Class
{
    
    public class YoutubeClass : IYoutube
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostingEnvironment;
        private SqlConnection sql;
        private SqlConnection sql1;

        private readonly string APIKEY;
        private readonly string BaseURL;
        private readonly string part;
        public YoutubeClass(IConfiguration _config, IWebHostEnvironment _hostingEnvironment)
        {
            config = _config;
            hostingEnvironment = _hostingEnvironment;
            sql = new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
            sql1 = new SqlConnection(config["ConnectionStrings:YouTubeConnection"]);
            APIKEY = config["YouTube:API_KEY"];
            BaseURL = config["YouTube:BASE_URI"];
            part = config["YouTube:part"];

        }
        public async Task<ServiceResponseT<object>> getData(string searchQuery)
        {
            string type = "search";
            var response = new ServiceResponseT<object>();
            try 
            {
               
                Uri url = new Uri(string.Format(BaseURL+ type +"?part="+part+"&key="+APIKEY+"&q="+searchQuery+"&maxResults="+20));
                string response1 = string.Empty;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.Method = "GET";
                request.ContentType = "application/json";
                StreamReader rsps = new StreamReader(request.GetResponse().GetResponseStream());
                response.ResponseCode = 200;
                response.Message = "Successfully Request to API";
                response.Data = JsonConvert.DeserializeObject<VideoList>(rsps.ReadToEnd().ToString());

            }
            catch(WebException e)
            {
                string pageContent = new StreamReader(e.Response.GetResponseStream()).ReadToEnd().ToString();
                response.ResponseCode = 400;
                response.Message = "Web Exception Found : " + e.Message; ;
                response.Data = pageContent;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = "Exception Found";
                response.Data = ex.Message;
            }

            return response;
        }
        public async Task<ServiceResponseT<object>> getPlaylist()
        {
            var service = new ServiceResponseT<object>();
            try {
                string type = "Get";
                DynamicParameters params1 = new DynamicParameters();
                params1.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);
                params1.Add("@type", type, dbType: DbType.String, direction: ParameterDirection.Input);
                var Result = await sql1.QueryAsync<dynamic>("usp_Playlist", params1, commandType: CommandType.StoredProcedure);
                var retval = params1.Get<int>("retval");

                service.ResponseCode = 200;

                if (retval == 10)
                {
                    service.Message = "Sucessfully Get playlist";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Result = Result
                    };
                }
                else
                {
                    service.Message = "No Queued Songs";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Message = "No Queued Songs"

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
        public async Task<ServiceResponseT<object>> addPlaylist(AddPlaylist add)
        {
            var service = new ServiceResponseT<object>();
            try
            {
                string type = "Add";
                DynamicParameters params1 = new DynamicParameters();
                params1.Add("@Link", add.link, dbType: DbType.String, direction: ParameterDirection.Input);
                params1.Add("@Ecode", add.ecode, dbType: DbType.String, direction: ParameterDirection.Input);
                params1.Add("@Artist", add.artist, dbType: DbType.String, direction: ParameterDirection.Input);
                params1.Add("@Title", add.title, dbType: DbType.String, direction: ParameterDirection.Input);
                params1.Add("@type", type,dbType: DbType.String, direction: ParameterDirection.Input);
                params1.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var Result = await sql1.QueryAsync<dynamic>("usp_Playlist", params1, commandType: CommandType.StoredProcedure);

                var retval = params1.Get<int>("retval");

                service.ResponseCode = 200;

                if (retval == 10)
                {
                    service.Message = "Sucessfully insert playlist";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Result = Result
                    };
                }
                else if(retval == 30)
                {
                    service.Message = "Queue Limit Reached";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Message = "Queue Limit Reached"

                    };
                }
                else if (retval == 40)
                {
                    service.Message = "Duplication of adding song limit reached within the day.";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Message = "Duplication Limit Reached"

                    };
                }
                else
                {
                    service.Message = "Insert Unsuccessful";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Message = "Insert Unsuccessful"

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

        public async Task<ServiceResponseT<object>> playSong()
        {
            var service = new ServiceResponseT<object>();
            try
            {
                string type = "Play";
                DynamicParameters params1 = new DynamicParameters();
                params1.Add("@type", type, dbType: DbType.String, direction: ParameterDirection.Input);
                params1.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);
                

                var Result = await sql1.QueryAsync<dynamic>("usp_Playlist", params1, commandType: CommandType.StoredProcedure);

                var retval = params1.Get<int>("retval");
                
                service.ResponseCode = 200;

                if (retval == 10)
                {
                    service.Message = "Successfully Request";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Message = "Song Link to be played.",
                        Result = Result
                    };
                }
                else if (retval == 30) 
                {
                    service.Message = "Successfully Request";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Message = "No Song on a Playlist",
                        Result = ""

                    };
                }
                else
                {
                    service.Message = " Successfully Request";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Message = "Unable to Get Any Song",
                         Result = ""

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

        public async Task<ServiceResponseT<object>> updateSong()
        {
            var service = new ServiceResponseT<object>();
            try
            {
                string type = "Update";
                DynamicParameters params1 = new DynamicParameters();
                params1.Add("@type", type, dbType: DbType.String, direction: ParameterDirection.Input);
                params1.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);


                var Result = await sql1.QueryAsync<dynamic>("usp_Playlist", params1, commandType: CommandType.StoredProcedure);

                var retval = params1.Get<int>("retval");

                service.ResponseCode = 200;

                if (retval == 10)
                {
                    service.Message = "Successfully Request";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Message = "Proceeding to Next Song",
                        Result = Result
                    };
                }
                else if (retval == 30)
                {
                    service.Message = "Successfully Request";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Message = "No Playing Songs",
                        Result = ""

                    };
                }
                else
                {
                    service.Message = " Successfully Request";
                    service.Data = new
                    {
                        ResponseCode = retval,
                        Message = "Unable to Update to Finished",
                        Result = ""

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
