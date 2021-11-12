using DOTNET_REST_WEB_API.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class YoutubeController : BaseController
    {
        private readonly IWebHostEnvironment _hosting;
        private readonly IAdapterRepository _repost;
        public YoutubeController(IAdapterRepository repost, IWebHostEnvironment hosting)
        {
            _repost = repost;
            _hosting = hosting;
        }
        //Get List of Youtube Videos
        [HttpGet]
        [Route("GetListVideos/{query}")]
        public async Task<IActionResult> GetListVideos(string query)
        {
            var val = await _repost.yout.getData(query);
            return Ok(val);
        }
    }
}
