using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using ProjectShoeShop.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.UI;

namespace ProjectShoeShop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        ShoeShopDbContext db = new ShoeShopDbContext();
        // GET: Admin/User
        public ActionResult ShowUsers(String Search = "", String IconClass = "fa-sort-asc",
            String SortColumn = "Id", int Page = 1)
        {
            try
            {
                List<User> listUsers = db.Users.ToList();
                if(listUsers != null)
                {
                    ViewBag.UsersNumber = listUsers.Count();

                    //Search
                    if (!string.IsNullOrEmpty(Search))
                    {
                        listUsers = db.Users.Where(m => m.UserName.Contains(Search)).ToList();
                        ViewBag.Search = Search;
                    }

                    //sort
                    ViewBag.IconClass = IconClass;
                    ViewBag.SortColumn = SortColumn;
                    if (SortColumn == "FullName")
                    {
                        if (IconClass == "fa-sort-asc")
                        {
                            listUsers = listUsers.OrderBy(m => m.FullName).ToList();
                        }
                        else
                        {
                            listUsers = listUsers.OrderByDescending(m => m.FullName).ToList();
                        }
                    }
                    else if (SortColumn == "UserName")
                    {
                        if (IconClass == "fa-sort-asc")
                        {
                            listUsers = listUsers.OrderBy(m => m.UserName).ToList();
                        }
                        else
                        {
                            listUsers = listUsers.OrderByDescending(m => m.UserName).ToList();
                        }
                    }

                    //Paging
                    int NumberOfRecords = 10;
                    int NumberOfPages = Convert.ToInt32(Math.Ceiling(listUsers.Count / (1.0 * NumberOfRecords)));
                    int NumberOfRecordToSkip = (Page - 1) * NumberOfRecords;
                    listUsers = listUsers.Skip(NumberOfRecordToSkip).Take(NumberOfRecords).ToList();
                    ViewBag.NumberOfPages = NumberOfPages;
                    ViewBag.Page = Page;

                    return View(listUsers);
                }
                else
                {
                    return HttpNotFound();
                }
            } catch(Exception ex)
            {
                ViewBag.ErrorMessage = "Lỗi khi load dữ liệu user";
                return View();
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(RegisterVM regis, HttpPostedFileBase ImagePath)
        {
            if (ModelState.IsValid)
            {
                var chkUser = db.Users.Where(x => x.UserName == regis.UserName).FirstOrDefault();
                if (chkUser == null)
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

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImagePath.FileName);

                        var filePath = Path.Combine(imagesFolder, fileName);
                        ImagePath.SaveAs(filePath);

                        user.ImagePath = "/Assets/images/user/" + fileName;
                    }

                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("ShowUsers");
                }
                else
                {
                    ViewBag.Message = "Tên username đã tồn tại !";
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            RegisterVM user = db.Users
                     .Where(u => u.Id == id)
                     .Select(u => new RegisterVM()
                     {
                         FullName = u.FullName,
                         UserName = u.UserName,
                         Password = u.PasswordHash,
                         ConfirmPassword = u.PasswordHash,
                         Email = u.Email,
                         Phone = u.Phone,
                         Birth = u.Birth,
                         Address = u.Address,
                         ImagePath = u.ImagePath,
                     }).FirstOrDefault();
            if (user != null)
            {
                ViewBag.IdUser = id;
                ViewBag.currentImg = user.ImagePath;
                return View(user);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(RegisterVM regis, HttpPostedFileBase ImagePath, string id)
        {
            if(ModelState.IsValid)
            {
                var user = db.Users.Where(m => m.Id == id).FirstOrDefault();
                if (user != null && !db.Users.Where(m => m.Id != id).Any(m => m.UserName == regis.UserName))
                {
                    user.UserName = regis.UserName;
                    user.FullName = regis.FullName;
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(regis.Password);
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
                    return RedirectToAction("ShowUsers");
                }
                else
                {
                    ViewBag.Message = "Username đã tồn tại !";
                    return View();
                }
            }
            return View();
        }
        public ActionResult Delete(string id)
        {
            try
            {
                RegisterVM user = db.Users
                     .Where(u => u.Id == id)
                     .Select(u => new RegisterVM()
                     {
                         FullName = u.FullName,
                         UserName = u.UserName,
                         Password = u.PasswordHash,
                         ConfirmPassword = u.PasswordHash,
                         Email = u.Email,
                         Phone = u.Phone,
                         Birth = u.Birth,
                         Address = u.Address,
                         ImagePath = u.ImagePath,
                     }).FirstOrDefault();
                ViewBag.IdUser = id;
                ViewBag.currentImg = user.ImagePath;

                return View(user);
            }
            catch
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(string id)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                db.Users.Remove(user);

                db.SaveChanges();
            }

            return RedirectToAction("ShowUsers");
        }
        public ActionResult Detail(string id)
        {
            RegisterVM user = db.Users
                    .Where(u => u.Id == id)
                    .Select(u => new RegisterVM()
                    {
                        FullName = u.FullName,
                        UserName = u.UserName,
                        Password = u.PasswordHash,
                        ConfirmPassword = u.PasswordHash,
                        Email = u.Email,
                        Phone = u.Phone,
                        Birth = u.Birth,
                        Address = u.Address,
                        ImagePath = u.ImagePath,
                    }).FirstOrDefault();
            if (user != null)
            {
                ViewBag.currentImg = user.ImagePath;
                return View(user);
            }
            return View();
        }
    }
}