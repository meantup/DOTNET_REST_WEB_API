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

namespace DOTNET_REST_WEB_API.Class
{
    
    public class YoutubeClass : IYoutube
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostingEnvironment;
        private SqlConnection sql;

        private readonly string APIKEY;
        private readonly string BaseURL;
        private readonly string part;
        public YoutubeClass(IConfiguration _config, IWebHostEnvironment _hostingEnvironment)
        {
            config = _config;
            hostingEnvironment = _hostingEnvironment;
            sql = new SqlConnection(config["ConnectionStrings:DefaultConnection"]);
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
    }
}
