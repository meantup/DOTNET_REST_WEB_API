using DOTNET_REST_WEB_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Repository
{
    public interface IDefault
    {
        Task<string> path(IFormFile image);
        Task<ServiceResponseT<object>> insert(InsertProduct insert);
        Task<ServiceResponseT<List<OrderList>>> inquiryList(string from, string to);
    }
}
