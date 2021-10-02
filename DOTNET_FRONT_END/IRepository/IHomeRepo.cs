using DOTNET_FRONT_END.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_FRONT_END.IRepository
{
    public interface IHomeRepo
    {
        Task<ServiceResponse<dynamic>> getLogin(string username,string pass);
    }
}
