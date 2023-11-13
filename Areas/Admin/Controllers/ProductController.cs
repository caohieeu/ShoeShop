using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using ProjectShoeShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectShoeShop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ShoeShopDbContext db = new ShoeShopDbContext();

        // GET: Admin/Product

        public ActionResult Index()
        {
            var list = db.Products.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var category = db.Categories.ToList(); 
            var brand = db.Brands.ToList();
            ProductVM productVM = new ProductVM()
            {
                CategoryList = category.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                BrandList = brand.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Product = new Product(),
            };

            return View(productVM);
        }

        [HttpPost]
        public ActionResult Create(ProductVM obj)
        {
            
                if (ModelState.IsValid)
                {
                    db.Products.Add(obj.Product);
                    db.SaveChanges();
                }
            
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            
            var category = db.Categories.ToList();
            var brand = db.Brands.ToList();
            ProductVM productVM = new ProductVM()
            {
                CategoryList = category.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                BrandList = brand.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Product = db.Products.FirstOrDefault(u => u.Id == id)
        };
            return View(productVM);
        }

        [HttpPost]
        public ActionResult Edit(ProductVM obj)
        {

            if (ModelState.IsValid)
            {
                var existingProduct = db.Products.FirstOrDefault(x => x.Id == obj.Product.Id);

                if (existingProduct != null)
                {
                    existingProduct.Name = obj.Product.Name;
                    existingProduct.Description = obj.Product.Description;
                    existingProduct.ImageURL = obj.Product.ImageURL;
                    existingProduct.Price = obj.Product.Price;
                    existingProduct.Stock = obj.Product.Stock;
                    existingProduct.BrandID = obj.Product.BrandID;
                    existingProduct.CategoryID = obj.Product.CategoryID;

                    db.SaveChanges();
                }

                
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var product = db.Products.FirstOrDefault(u => u.Id == id);
            return View(product);
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(string id)
        {
            var existingProduct = db.Products.FirstOrDefault(x => x.Id == id);

            if (existingProduct != null)
            {
                db.Products.Remove(existingProduct);

                db.SaveChanges();
            }

            return RedirectToAction("Index");

        }
    }
}