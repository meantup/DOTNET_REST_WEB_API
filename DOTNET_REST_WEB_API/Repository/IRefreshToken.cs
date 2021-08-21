using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Repository
{
    public interface IRefreshToken
    {
        string GenerateToken(string secretkey, string issuer, string audience, double expirationMinutes);
    }
}
