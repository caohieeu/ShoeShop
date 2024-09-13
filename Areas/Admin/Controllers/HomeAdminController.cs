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
            ViewBag.TotalUser = TotalUser();
            ViewBag.TotalIncome = TotalIncome();
            ViewBag.TotalProductSold = TotalProductSold();
            ViewBag.TotalProductRemain = TotalProductRemain();
            return View();
        }
        public int CountComment()
        {
            return db.Reviews.Count();
        }
        public double CountPositive()
        {
            try
            {
                return (db.Reviews.Where(t => t.Rating >= 3).Count()) * 100 / CountComment();
            }
            catch { return 100; }

        }
        public double CountNegative()
        {
            try
            {
                return (db.Reviews.Where(t => t.Rating < 3).Count()) * 100 / CountComment();
            }
            catch { return 0; }
        }
        public ActionResult TopUser()
        {
            var lst = db.Users.Select(t => new RankUserVM
            {
                Id = t.Id,
                FullName = t.FullName,
                Phone = t.Phone,
                Email = t.Email,
                ImagePath = t.ImagePath,
                ToTalMoney = (db.Orders
                .Where(o => o.User.Id == t.Id)
                .Join(db.OrderDetails, o => o.Id, od => od.OrderId, (o, od) => new { Order = o, OrderDetail = od })
                .Sum(merged => (double?)merged.OrderDetail.Quantity * merged.OrderDetail.Price)) ?? 0
            }).OrderByDescending(t => t.ToTalMoney).Take(5).ToList();
            return View(lst);
        }
        public ActionResult NewOrders()
        {
            var lst = from user in db.Users
                        join order in db.Orders on user.Id equals order.UserID
                        join orderDetail in db.OrderDetails on order.Id equals orderDetail.OrderId
                        join product in db.Products on orderDetail.ProductId equals product.Id
                        select new NewOrdersVM
                        {
                            Id = user.Id,
                            FullName = user.FullName,
                            ImagePath = user.ImagePath,
                            QuantityProduct = orderDetail.Quantity,
                            DeliveryStatus = order.DeliveryStatus,
                            ProductId = product.Id,
                            NameProduct = product.Name
                        };
            var st = lst.Take(5).ToList();
            return View(st);
        }
        public int TotalUser()
        {
            try
            {
                var lst = db.Users.Count();
                return lst;
            }
            catch { return 0; }
        }
        public double TotalIncome()
        {
            try {
                var lst = db.Users.Select(t => new RankUserVM
                {
                    Id = t.Id,
                    FullName = t.FullName,
                    Phone = t.Phone,
                    Email = t.Email,
                    ImagePath = t.ImagePath,
                    ToTalMoney = (db.Orders
                    .Where(o => o.User.Id == t.Id)
                    .Join(db.OrderDetails, o => o.Id, od => od.OrderId, (o, od) => new { Order = o, OrderDetail = od })
                    .Sum(merged => (double?)merged.OrderDetail.Quantity * merged.OrderDetail.Price)) ?? 0
                });
                double sum = lst.Sum(t => t.ToTalMoney);
                return sum;
            }
            catch { return 0; }
        }
        public int TotalProductSold()
        {
            try
            {
                var lst = db.OrderDetails.Sum(t => t.Quantity);
                return lst;
            }
            catch { return 0; }
        }
        public int TotalProductRemain()
        {
            try
            {
                var lst = db.Products.Sum(t => t.Stock);
                return lst;
            }
            catch { return 0; }
        }
    }
}