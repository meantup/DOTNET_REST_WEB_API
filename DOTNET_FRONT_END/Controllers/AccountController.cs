using DOTNET_FRONT_END.IRepository;
using DOTNET_FRONT_END.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_FRONT_END.Controllers
{
    public class AccountController : Controller
    {
        private readonly AdapterRepository _repos;
        public AccountController(AdapterRepository repos)
        {
            _repos = repos;
        }
        public IActionResult AccountLogin()
        {
            return View();
        }
        public async Task<ActionResult> Index(string user, string pass)
        {
            var res = await _repos.repoHome.getLogin(user, pass);
            if (res.responseCode == 200)
            {
                return RedirectToAction("RedirectlyHome", "Home");
            }
            else if (res.responseCode == 500 || res.responseCode == 501)
            {
                Response.StatusCode = 201;
                return Json(new ErrorJson { title = res.Message, type = "error", message = res.Data.ToString(), status = 30 });
            }
            else
            {
                Response.StatusCode = 201;
                return Json(new ErrorJson { title = "Oops!", type = "warning", message = res.Data.data.message, status = 30 });
            }

        }
    }
}
