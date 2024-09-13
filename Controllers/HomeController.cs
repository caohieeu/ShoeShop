using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;

namespace ProjectShoeShop.Controllers
{
    public class HomeController : Controller
    {
        ShoeShopDbContext db = new ShoeShopDbContext();
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                var listProduct = db.Products.Include(nameof(Product.Brand))
                                         .Include(nameof(Product.Category))
                                         .Take(6).ToList();
                return View(listProduct);
            }
            catch
            {
                return HttpNotFound();
            }
        }
    }
}