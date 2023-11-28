using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ProjectShoeShop.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        // GET: Admin/Brand
        ShoeShopDbContext db = new ShoeShopDbContext();
        public ActionResult Index(String Search = "", String IconClass = "fa-sort-asc",
            String SortColumn = "Name", int Page = 1)
        {
            var listBrands = db.Brands.ToList();
            if(listBrands != null)
            {
                ViewBag.BrandsNumber = listBrands.Count;

                //Search
                if (!string.IsNullOrEmpty(Search))
                {
                    listBrands = db.Brands.Where(m => m.Name.Contains(Search)).ToList();
                    ViewBag.Search = Search;
                }

                //sort
                ViewBag.IconClass = IconClass;
                ViewBag.SortColumn = SortColumn;
                if (SortColumn == "Name")
                {
                    if (IconClass == "fa-sort-asc")
                    {
                        listBrands = listBrands.OrderBy(m => m.Name).ToList();
                    }
                    else
                    {
                        listBrands = listBrands.OrderByDescending(m => m.Name).ToList();
                    }
                }

                //Paging
                int NumberOfRecords = 7;
                int NumberOfPages = Convert.ToInt32(Math.Ceiling(listBrands.Count / (1.0 * NumberOfRecords)));
                int NumberOfRecordToSkip = (Page - 1) * NumberOfRecords;
                listBrands = listBrands.Skip(NumberOfRecordToSkip).Take(NumberOfRecords).ToList();
                ViewBag.NumberOfPages = NumberOfPages;
                ViewBag.Page = Page;

                return View(listBrands);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Brand obj)
        {
            var brand = obj;
            if(brand != null)
            {
                if(db.Brands.Any(m => m.Name == brand.Name))
                {   
                    ViewBag.Message = "Tên đã tồn tại !";
                    return View();
                }
                brand.Id = Guid.NewGuid().ToString().Substring(0, 7);
                db.Brands.Add(brand);
                db.SaveChanges();
            }

            return RedirectToAction("Index");   
        }

        public ActionResult Edit(string id)
        {
            Brand brand = db.Brands.FirstOrDefault(x => x.Id == id);
            ViewBag.IdBrand = brand.Id;
            return View(brand); 
        }

        [HttpPost]
        public ActionResult Edit(Brand obj, string id)
        {
            var existingBrand = db.Brands.FirstOrDefault(x => x.Id == id);

            if (existingBrand != null)
            {
                if(db.Brands.Where(m => m.Id != id)
                            .Any(m => m.Name == obj.Name))
                {
                    ViewBag.Message = "Tên đã tồn tại !";
                    return View();
                }
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