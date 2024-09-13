using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectShoeShop.DAL;
using ProjectShoeShop.ViewModel;
using ProjectShoeShop.Models;
using System.Data.Entity;
using System.Drawing.Printing;
using Microsoft.SqlServer.Server;
using System.Web.WebPages;
using ProjectShoeShop.Services;
using System.IO;
using System.Web.UI;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ProjectShoeShop.Controllers
{
    public class AccountController : Controller
    {
        ShoeShopDbContext db = new ShoeShopDbContext();
        // GET: Account
        public ActionResult AccountPartialView()
        {
            return PartialView();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterVM regis, HttpPostedFileBase ImagePath)
        {
            if (ModelState.IsValid)
            {
                var chkUser = db.Users.Where(x => x.UserName == regis.UserName).FirstOrDefault();
                if(chkUser == null)
                {
                   var user = new User();
                    user.FullName = regis.FullName;
                    user.UserName = regis.UserName;
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(regis.Password);
                    user.Email = regis.Email;
                    user.Birth = regis.Birth;
                    user.Address = regis.Address;
                    user.Phone = regis.Phone;
                    user.RoleId = db.Roles.Where(x => x.Name == utility.Customer).Select(x => x.Id).FirstOrDefault();

                    if (ImagePath != null && ImagePath.ContentLength > 0)
                    {
                        var imagesFolder = Server.MapPath("~/Assets/images/user");

                        if (!Directory.Exists(imagesFolder))
                        {
                            Directory.CreateDirectory(imagesFolder);
                        }

                        //if(!string.IsNullOrEmpty(chkUser?.ImagePath))
                        //{
                        //    var oldImage = Server.MapPath(chkUser.ImagePath);
                        //    if(System.IO.File.Exists(oldImage))
                        //    {
                        //        System.IO.File.Delete(oldImage);
                        //    }
                        //}

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImagePath.FileName);

                        var filePath = Path.Combine(imagesFolder, fileName);
                        ImagePath.SaveAs(filePath);

                        user.ImagePath = "/Assets/images/user/" + fileName;
                    }

                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ViewBag.Message = "Tên username đã tồn tại !";
                    return View();
                }
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM login)
        {
            if(ModelState.IsValid)
            {
                UserService userService = new UserService();
                var user = userService.Login(login.UserName, login.Password);
                if (user != null)   
                {
                    Session["User"] = user as User;
                    User us = (User)Session["User"];
                    if(us.UserName == "admin")
                    {
                        return RedirectToAction("Index", "HomeAdmin", new {area = "Admin"});
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Nhập sai tên hoặc mật khẩu !";
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("User");
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Information()
        {
            try
            {
                User u = (User)Session["User"];
                User user = db.Users.Where(x => x.Id == u.Id).FirstOrDefault();
                RegisterVM inforUser = new RegisterVM()
                {
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Password = user.PasswordHash,
                    Email = user.Email,
                    Phone = user.Phone,
                    Birth = user.Birth,
                    ImagePath = user.ImagePath,
                    Address = user.Address,
                };
                if (inforUser != null)
                {
                    ViewBag.currentImg = user.ImagePath;
                    return View(inforUser);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch
            {
                return HttpNotFound();   
            }
        }
        [HttpPost]
        public ActionResult Information(RegisterVM regis, HttpPostedFileBase ImagePath)
        {
            regis.Password = regis.ConfirmPassword;
            if (!ModelState.IsValid)
            {
                User u = (User)Session["User"];
                User user = db.Users.Where(x => x.Id == u.Id).FirstOrDefault();
                if (user != null && !db.Users.Where(m => m.Id != user.Id).Any(m => m.UserName == regis.UserName))
                {
                    user.UserName = regis.UserName;
                    user.FullName = regis.FullName;
                    user.Email = regis.Email;
                    user.Birth = regis.Birth;
                    user.Address = regis.Address;
                    user.Phone = regis.Phone;

                    if (ImagePath != null && ImagePath.ContentLength > 0)
                    {
                        if (!string.IsNullOrEmpty(user.ImagePath))
                        {
                            var oldImagePath = Server.MapPath(user.ImagePath);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        var imagesFolder = Server.MapPath("~/Assets/images/user");

                        if (!Directory.Exists(imagesFolder))
                        {
                            Directory.CreateDirectory(imagesFolder);
                        }

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImagePath.FileName);

                        var filePath = Path.Combine(imagesFolder, fileName);
                        ImagePath.SaveAs(filePath);

                        user.ImagePath = "/Assets/images/user/" + fileName;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Username đã tồn tại !";
                    return View();
                }
            }
            return View();
        }
        public ActionResult Ordered()
        {
            try
            {
                User user = (User)Session["User"];
                if (user != null)
                {
                    List<OrderVM> order = db.Orders
                        .Where(x => x.UserID == user.Id)
                        .Select(x => new OrderVM
                        {
                            OrderId = x.Id,
                            ProductId = db.OrderDetails
                                            .Where(m => m.OrderId == x.Id)
                                            .Select(m => m.Product.Id).FirstOrDefault(),
                            ImageProduct = db.OrderDetails
                                            .Where(m => m.OrderId == x.Id)
                                            .Select(m => m.Product.ImageURL).FirstOrDefault(),
                            NameProduct = db.OrderDetails
                                            .Where(m => m.OrderId==x.Id)
                                            .Select(m => m.Product.Name).FirstOrDefault(),
                            QuantityProduct = db.OrderDetails
                                                .Where(m => m.OrderId == x.Id)
                                                .Sum(m => m.Quantity),
                            DeliveryStatus = x.DeliveryStatus,
                            TotalAmount = db.OrderDetails
                                                .Where(m => m.OrderId == x.Id)
                                                .Sum(m => m.Price),
                        }).ToList();
                    return View(order);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch
            {
                return HttpNotFound();
            }
        }
        public ActionResult ConfirmReceived(string OrderId)
        {
            try
            {
                Order order = db.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
                if(order != null)
                {
                    order.DeliveryStatus = "Delivered";
                    ViewBag.OrderId = OrderId;
                    db.SaveChanges();
                    return View();
                }
                else
                {
                    return RedirectToAction("Ordered");
                }
            }
            catch
            {
                return HttpNotFound();
            }
        }
        public ActionResult ReviewPartial(string productID)
        {
            var lst = db.Reviews.Include(nameof(Review.User))
                    .Include(nameof(Review.Product)).Where(t => t.ProductId == productID)
                    .Select(t => new ReviewVM
                    {
                        FullName = t.User.FullName,
                        ImagePath = t.User.ImagePath,
                        Comment = t.Comment,
                        Rating = t.Rating,
                        ReviewDate = t.ReviewDate
                    })
                    .ToList();
            if (lst != null)
            {
                return PartialView(lst);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult ReviewProduct(string OrderId)
        {
            string ProductId = db.OrderDetails
                                .Where(x => x.OrderId == OrderId)
                                .Select(x => x.Product.Id).FirstOrDefault();
            ViewBag.ProductId = ProductId;
            ViewBag.OrderId = OrderId;
            return View();
        }
        [HttpPost]
        public ActionResult ReviewProduct(FormCollection f, string ProductId, string OrderId)
        {
            User user = (User)Session["User"];
            Review review = new Review()
            {
                Id = Guid.NewGuid().ToString().Substring(0, 7),
                ProductId = ProductId,
                UserId = user.Id,
                Rating = int.Parse(f["Rating"]),
                Comment = f["Comment"].ToString(),
                ReviewDate = DateTime.Now
            };
            Order order = db.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
            order.DeliveryStatus = "Success";
            db.SaveChanges();

            db.Reviews.Add(review);
            db.SaveChanges();
            return RedirectToAction("Ordered", "Account");
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(PasswordVM passwordObj)
        {
            if(ModelState.IsValid)
            {
                User user = (User)Session["User"];
                if (!BCrypt.Net.BCrypt.Verify(passwordObj.OldPassword, user.PasswordHash))
                {
                    ModelState.AddModelError("", "Old password is incorrect !");
                    return View();
                }
                else
                {
                    var u = db.Users.Where(x => x.Id == user.Id).FirstOrDefault();
                    if (u != null)
                    {
                        u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordObj.NewPassword);
                        db.SaveChanges();
                    }
                    //ViewBag.Message = "Changed password successfully";
                }
                return RedirectToAction("Information");
            }
            return View();  
        }
    }
}