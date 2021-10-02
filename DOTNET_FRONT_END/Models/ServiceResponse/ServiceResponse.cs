using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_FRONT_END.Models
{
    public class ServiceResponse<T>
    {
        public int responseCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
    public class BaseDataToken
    {
        public object access_token { get; set; }
        public string expire_in { get; set; }
        public string token_type { get; set; }
    }
    public class ErrorJson
    {
        public string title { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public int status { get; set; }
    }
}
