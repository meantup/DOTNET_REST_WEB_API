using DOTNET_REST_WEB_API.Repository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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

        [SwaggerOperation(Summary = "Token Request for accessing API.", Description = "Enter your account credentials to get token.")]
        [HttpGet("LoginPlayer/{username}/{password}")]
        public async Task<IActionResult> loginPlayer(string username, string password)
        {
            var res = await repos.auth.RequestTokenPlayer(username, password);
            if (res.data != null)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

    }
}
