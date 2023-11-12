using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectShoeShop.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        // GET: Admin/Brand
        ShoeShopDbContext db = new ShoeShopDbContext();
        public ActionResult Index()
        {
            var list = db.Brands.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            

            return View();
        }

        [HttpPost]
        public ActionResult Create(Brand obj)
        {
            db.Brands.Add(obj);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var brand = db.Brands.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpPost]
        public ActionResult Edit(Brand obj)
        {
            var existingBrand = db.Brands.FirstOrDefault(x => x.Id == obj.Id);

            if (existingBrand != null)
            {
                existingBrand.Name = obj.Name;
                existingBrand.Description = obj.Description;

                db.SaveChanges(); 
            }

            return RedirectToAction("Index");

        }

        public ActionResult Delete(string id)
        {
            var brand = db.Brands.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(string id)
        {
            var existingBrand = db.Brands.FirstOrDefault(x => x.Id == id);

            if (existingBrand != null)
            {
                db.Brands.Remove(existingBrand);

                db.SaveChanges();
            }

            return RedirectToAction("Index");

        }
    }
}