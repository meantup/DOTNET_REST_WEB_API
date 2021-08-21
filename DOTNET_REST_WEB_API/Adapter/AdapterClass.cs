using DOTNET_REST_WEB_API.Class;
using DOTNET_REST_WEB_API.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Adapter
{
    public class AdapterClass : IAdapterRepository
    {
        public IAuthManager auth { get; }

        public IRefreshToken refresh { get; }

        public AdapterClass(IConfiguration config)
        {
            auth = new AuthManager(config);
            refresh = new RefreshToken(config);
        }
       
    }
}
