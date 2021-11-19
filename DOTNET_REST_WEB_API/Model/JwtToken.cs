using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Model
{
    public class JwtToken
    {
        public long exp { get; set; }
    }
    public class LoginAuth
    { 
       public string username { get; set; }
       public string password { get; set; }
    }

}
