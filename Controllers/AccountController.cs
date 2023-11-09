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
        public ActionResult Register(RegisterVM regis)
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
    }
}