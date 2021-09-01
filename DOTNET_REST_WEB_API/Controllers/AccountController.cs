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
        public AccountController()
        {

        }
        [HttpGet]
        [Route("Credential/SignUp")]
        public async Task<IActionResult> SigningUp()
        {
            return Ok();
        }
    }
}
