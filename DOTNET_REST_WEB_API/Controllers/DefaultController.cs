using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class DefaultController : BaseController
    {
        public DefaultController()
        {

        }
        [HttpGet]
        [Route("GetUserByID")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
