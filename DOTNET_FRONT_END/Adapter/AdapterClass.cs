using DOTNET_FRONT_END.Class;
using DOTNET_FRONT_END.IRepository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_FRONT_END.Adapter
{
    public class AdapterClass : AdapterRepository
    {
        private static string token { get; set; }
        public IHomeRepo repoHome { get; }
        public AdapterClass(IConfiguration _config)
        {
            repoHome = new HomeClass(_config);
        }
    }
}
