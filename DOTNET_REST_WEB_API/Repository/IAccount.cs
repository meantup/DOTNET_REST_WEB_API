using DOTNET_FRONT_END.Models;
using DOTNET_REST_WEB_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Repository
{
    public interface IAccount
    {
        Task<ServiceResponseT<object>> signUp(SignUp creds);
        Task<ServiceResponseT<object>> getUserList();
    }
}
