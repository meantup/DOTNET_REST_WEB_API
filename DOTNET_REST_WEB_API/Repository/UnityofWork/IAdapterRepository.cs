using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Repository
{
    public interface IAdapterRepository
    {
        IAuthManager auth { get; }
        IRefreshToken refresh { get; }
        IDefault def { get;}
        IRepository repo { get; }
    }
}
