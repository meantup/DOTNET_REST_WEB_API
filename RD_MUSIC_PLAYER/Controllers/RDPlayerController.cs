using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RD_MUSIC_PLAYER.Controllers
{
    public class RDPlayerController : Controller
    {
        // GET: RDPlayer
        public ActionResult Index()
        {
            return View();
        }
    }
}