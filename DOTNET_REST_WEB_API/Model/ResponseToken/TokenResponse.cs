using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Model
{
    public class TokenResponse
    {
        public string access_token { get; set; }
        public string expire_in { get; set; }
        public string token_type { get; set; }
    }
}
