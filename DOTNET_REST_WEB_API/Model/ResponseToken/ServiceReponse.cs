using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Model
{
    public class ServiceResponse<T>
    {
        public T data { get; set; }
    }
    public class ServiceResponseT<T>
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
