using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Model
{
    public class TokenResponse<T>
    {
        public T token_Type { get; set; }
        public int token_Expired { get; set; }
        public string token { get; set; }
    }
}
