using DOTNET_REST_WEB_API.Model;
using DOTNET_REST_WEB_API.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class DefaultController : BaseController
    {
        private readonly IWebHostEnvironment _hosting;
        private readonly IAdapterRepository _repost;
        public DefaultController(IAdapterRepository repost, IWebHostEnvironment hosting)
        {
            _repost = repost;
            _hosting = hosting;
        }
        [HttpPost]
        [Route("InsertRecord")]
        public async Task<IActionResult> InsertRecord(IFormFile file, [FromForm] InsertProduct product)
        {
            try
            {
                var fileName = string.Empty;
                if (file.Length > 0)
                {
                    var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    fileName = Guid.NewGuid().ToString() + extension;
                  
                    var filePath = Path.Combine(_hosting.WebRootPath, "Images\\", fileName);

                    using (var bits = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(bits);
                        product.filepath = $"{this.Request.Scheme}://{this.Request.Host}/Images/{fileName}";
                    }
                    var ret = await _repost.def.insert(product);
                    return Ok(ret);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("GetInquiry/{from}/{to}")]
        public async Task<IActionResult> Inquiry(string from,string to)
        {
            var ret = await _repost.def.inquiryList(from, to);
            if (ret.ResponseCode.Equals(200))
            {
                return Ok(ret);
            }
            else
            {
                return Ok(ret);
            }
            return BadRequest();
        }

    }
}
