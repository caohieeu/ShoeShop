using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectShoeShop.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        ShoeShopDbContext db = new ShoeShopDbContext();
        public ActionResult Index()
        {
            var list = db.Categories.ToList();
            return View(list);
        }

        public ActionResult Create()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Create(Category obj)
        {
            db.Categories.Add(obj);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var category = db.Categories.FirstOrDefault(x => x.Id == id);

            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category obj)
        {
            var existingCategory = db.Categories.FirstOrDefault(x => x.Id == obj.Id);

            if (existingCategory != null)
            {
                existingCategory.Name = obj.Name;
                existingCategory.Description = obj.Description;

                db.SaveChanges();
            }

            return RedirectToAction("Index");

        }

        public ActionResult Delete(string id)
        {
            var Category = db.Categories.FirstOrDefault(x => x.Id == id);

            return View(Category);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(string id)
        {
            var existingCategory = db.Categories.FirstOrDefault(x => x.Id == id);

            if (existingCategory != null)
            {
                db.Categories.Remove(existingCategory);

                db.SaveChanges();
            }

            return RedirectToAction("Index");

        }
    }
}