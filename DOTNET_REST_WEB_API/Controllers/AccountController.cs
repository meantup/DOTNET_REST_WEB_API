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
    public class AccountController : ControllerBase
    {
        private readonly IAdapterRepository repos;
        public AccountController(IAdapterRepository _repost)
        {
            repos = _repost;
        }
        [HttpGet("Login")]
        public async Task<IActionResult> login(string password, string username)
        {
            var res = await repos.auth.RequestToken(password, username);
            if (res.data != null)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
