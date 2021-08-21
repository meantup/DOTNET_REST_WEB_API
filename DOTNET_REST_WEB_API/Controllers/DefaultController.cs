using DOTNET_REST_WEB_API.Model;
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
        private readonly IWebHostEnvironment _hostingEnvironment;
        public DefaultController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        [Route("GetUserByID")]
        public IActionResult Index()
        {
            return Ok();
        }
        [HttpPost]
        [Route("InsertRecord")]
        public async Task<IActionResult> InsertRecord([FromForm] InsertProduct product)
        {
          
            if (product.image != null)
            {
                var a = _hostingEnvironment.WebRootPath;
                var fileName = Path.GetFileName(product.image.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images\\", fileName);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await product.image.CopyToAsync(fileSteam);
                }


                //product.image = filePath;  //save the filePath to database ImagePath field.
                //_context.Add(car);
                //await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
