using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectShoeShop.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult AboutLayout()
        {
            return View();
        }
    }
}