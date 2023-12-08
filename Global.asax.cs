using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using ProjectShoeShop.App_Start;
using Microsoft.Extensions.DependencyInjection;
using System.Web.SessionState;

namespace ProjectShoeShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            CreateRolesAndUser();
        }
        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }
        public void CreateRolesAndUser()
        {
            var db = new ShoeShopDbContext();
            if (db.Roles.Count() == 0)
            {
                List<Role> roles = new List<Role>()
                {
                    new Role() {Id=Guid.NewGuid().ToString().Substring(0, 7), Name=utility.Admin},
                    new Role() {Id=Guid.NewGuid().ToString().Substring(0, 7), Name=utility.Manager},
                    new Role() {Id=Guid.NewGuid().ToString().Substring(0, 7), Name=utility.Customer},
                };
                db.Roles.AddRange(roles);
                db.SaveChanges();
            }

            if (db.Users.Count() == 0)
            {
                var user = new User()
                {
                    UserName = "admin",
                    Address = "admin@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    RoleId = db.Roles.Where(x => x.Name == utility.Admin).Select(x => x.Id).FirstOrDefault(),
                    FullName = "admin"
                };
                db.Users.Add(user);
                db.SaveChanges();
            }
        }
    }
}
