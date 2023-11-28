using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectShoeShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ProductLayout()
        {
            return View();
        }
        public ActionResult ShowProduct()
        {
            return View();
        }
        public ActionResult DetailProduct()
        {
            return View();
        }
    }
}