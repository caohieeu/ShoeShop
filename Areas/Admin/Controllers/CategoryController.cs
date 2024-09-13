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
        public ActionResult Index(String Search = "", String IconClass = "fa-sort-asc",
            String SortColumn = "Name", int Page = 1)
        {
            var listCategories = db.Categories.ToList();

            if (listCategories != null)
            {
                ViewBag.CategoryNumber = listCategories.Count;
                //Search
                if (!string.IsNullOrEmpty(Search))
                {
                    listCategories = db.Categories.Where(m => m.Name.Contains(Search)).ToList();
                    ViewBag.Search = Search;
                }

                //sort
                ViewBag.IconClass = IconClass;
                ViewBag.SortColumn = SortColumn;
                if (SortColumn == "Name")
                {
                    if (IconClass == "fa-sort-asc")
                    {
                        listCategories = listCategories.OrderBy(m => m.Name).ToList();
                    }
                    else
                    {
                        listCategories = listCategories.OrderByDescending(m => m.Name).ToList();
                    }
                }
                //Paging
                int NumberOfRecords = 7;
                int NumberOfPages = Convert.ToInt32(Math.Ceiling(listCategories.Count / (1.0 * NumberOfRecords)));
                int NumberOfRecordToSkip = (Page - 1) * NumberOfRecords;
                listCategories = listCategories.Skip(NumberOfRecordToSkip).Take(NumberOfRecords).ToList();
                ViewBag.NumberOfPages = NumberOfPages;
                ViewBag.Page = Page;

                return View(listCategories);
            }
            return HttpNotFound();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category obj)
        {
            try
            {
                Category cate = obj;
                cate.Id = Guid.NewGuid().ToString().Substring(0, 7);
                db.Categories.Add(obj);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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