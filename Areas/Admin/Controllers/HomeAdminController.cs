using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using ProjectShoeShop.ViewModel;

namespace ProjectShoeShop.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        ShoeShopDbContext db = new ShoeShopDbContext();
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            ViewBag.CountComment = CountComment();
            ViewBag.CountPositive = CountPositive();
            ViewBag.CountNegative = CountNegative();
            //ViewBag.TotalOrder = ReviewsUser();
            return View();
        }
        public int CountComment()
        {
            return db.Reviews.Count();
        }
        public double CountPositive()
        {
            return (db.Reviews.Where(t=>t.Rating >= 3).Count()) * 100 / CountComment();
        }
        public double CountNegative()
        {
            return (db.Reviews.Where(t => t.Rating < 3).Count()) * 100 / CountComment();
        }
        //public List<RankUserVM> ReviewsUser()
        //{
        //    var lst = db.Users.Select(t => new RankUserVM
        //    {
        //        Id = t.Id,
        //        FullName = t.FullName,
        //        Phone = t.Phone,
        //        Email = t.Email,
        //        ImagePath = t.ImagePath,
        //        ToTalMoney = (double)(db.Orders
        //        .Where(o => o.User.Id == t.Id)
        //        .Join(db.OrderDetails, o => o.Id, od => od.OrderId, (o, od) => new { Order = o, OrderDetail = od })
        //        .Sum(merged => merged.OrderDetail.Quantity * merged.OrderDetail.Price))
        //    }).ToList();
        //    return lst;
        //}

    }
}