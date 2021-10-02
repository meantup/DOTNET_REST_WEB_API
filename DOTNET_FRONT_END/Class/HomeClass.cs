using DOTNET_FRONT_END.IRepository;
using DOTNET_FRONT_END.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DOTNET_FRONT_END.Class
{
    public class HomeClass : IHomeRepo
    {
        private readonly IConfiguration _config;
        private readonly string baseUrl;
        public static string token;
        public HomeClass(IConfiguration config) { _config = config; baseUrl = _config["BaseUrl:Uri"].Trim().ToString(); }
        public async Task<ServiceResponse<dynamic>> getLogin(string user,string pass)
        {
            var res = new ServiceResponse<dynamic>();
            try
            {
                Uri uri = new Uri(string.Format(baseUrl + _config["PrefixAuth:Login"] + "/{0}/{1}" , user.Trim().ToString(), pass.Trim().ToString()));

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                request.Method = "GET";
                request.ContentType = "application/json";
                StreamReader rsps = new StreamReader(request.GetResponse().GetResponseStream());
                res.Data = JsonConvert.DeserializeObject<object>(rsps.ReadToEnd());
                var getData = res.Data.data;
                var hasAccessToken = getData.access_token == null ? true : false;
                //res.Data.data.access_token = res.Data.data.access_token != null ? res.Data.data : token;
                token = hasAccessToken.Equals(true) ? getData.access_token : "";
                res.Message = "Success";
                res.responseCode = hasAccessToken.Equals(true) ? 201 : 200;
                return res;

            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    res.Data = reader.ReadToEnd();
                    res.responseCode = 500;
                    res.Message = "Web Exception Error!";
                    return res;
                    
                }
            }
            catch (Exception ex)
            {
                res.Data = ex.Message;
                res.responseCode = 501;
                res.Message = "Exception Error!";
                return res;
            }
        }
    }
}
