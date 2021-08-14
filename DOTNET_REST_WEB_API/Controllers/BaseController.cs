using DOTNET_REST_WEB_API.Helper;
using DOTNET_REST_WEB_API.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Controllers
{
    [Controller]
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        public UserInfo _user => (UserInfo)HttpContext.Items["User"];
    }
}
