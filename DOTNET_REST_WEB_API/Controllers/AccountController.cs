using DOTNET_FRONT_END.Models;
using DOTNET_REST_WEB_API.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountController : BaseController
    {
        private readonly IAdapterRepository repos;
        public AccountController(IAdapterRepository _repos)
        {
            repos = _repos;
        }
        [HttpPost]
        [Route("Credential/SignUp")]
        public async Task<IActionResult> SigningUp(SignUp creds)
        {
            var res = await repos.acc.signUp(creds);
            if (res.Data != null)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
        [HttpGet]
        [Route("UserList")]
        public async Task<IActionResult> GetUserList()
        {
            var res = await repos.acc.getUserList();
            if (res.Data != null)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
