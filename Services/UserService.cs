using ProjectShoeShop.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using BCrypt.Net;

namespace ProjectShoeShop.Services
{
    public class UserService
    {
        ShoeShopDbContext db = new ShoeShopDbContext();
        public dynamic Login(string username, string password)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == username);
            if(user == null)
                return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;
            return user;
        }
        //public dynamic Logout(string username, string password)
        //{

        //}
    }
}