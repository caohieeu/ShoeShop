using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectShoeShop.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult CartLayout()
        {
            return View();
        }
        public ActionResult ShowCart()
        {
            return View();
        }
    }
}