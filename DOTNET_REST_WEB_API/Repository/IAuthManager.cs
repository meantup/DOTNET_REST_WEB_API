using DOTNET_REST_WEB_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Repository
{
    public interface IAuthManager
    {
        Task<ServiceResponse<object>> RequestToken(string user, string pass);
    }
}
