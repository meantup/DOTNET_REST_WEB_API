using DOTNET_REST_WEB_API.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAdapterRepository repos;
        public AuthController(IAdapterRepository _repost)
        {
            repos = _repost;
        }
        [HttpGet("Login/{username}/{password}")]
        public async Task<IActionResult> login(string username, string password)
        {
            var res = await repos.auth.RequestToken(username, password);
            if (res.data != null)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

    }
}
