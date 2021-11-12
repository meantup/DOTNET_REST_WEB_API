using DOTNET_REST_WEB_API.Class;
using DOTNET_REST_WEB_API.Repository;
using Microsoft.AspNetCore.Hosting;
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
        public IDefault def { get; }

        public IRepository repo { get; }

        public IYoutube yout { get; }

        public AdapterClass(IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            auth = new AuthManager(config);
            refresh = new RefreshToken(config);
            def = new DefaultClass(config, hostingEnvironment);
            yout = new YoutubeClass(config, hostingEnvironment);
        }
       
    }
}
