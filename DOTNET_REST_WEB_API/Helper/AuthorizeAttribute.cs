using DOTNET_REST_WEB_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (UserInfo)context.HttpContext.Items["User"];
            if(user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }){ StatusCode = StatusCodes.Status401Unauthorized};
            }
        }
    }
}
