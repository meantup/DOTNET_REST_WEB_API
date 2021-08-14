using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Model
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string LoginId { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string Email { get; set; }
        public string sub { get; set; }
    }
}
