using DOTNET_REST_WEB_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Repository
{
    public interface IRepository
    {
        Task<ServiceResponse<object>> signin(string username,string password);
    }
}
