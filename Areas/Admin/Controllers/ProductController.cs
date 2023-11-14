using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using ProjectShoeShop.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult Create(ProductVM obj , HttpPostedFileBase imageFile)
        {
            

            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Id = obj.Product.Id,
                    Name = obj.Product.Name,
                    BrandID = obj.Product.BrandID,
                    CategoryID = obj.Product.CategoryID,
                    Description = obj.Product.Description,
                    Price = obj.Product.Price,
                    Size = obj.Product.Size,
                    Stock = obj.Product.Stock
                };
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var imagesFolder = Server.MapPath("~/Assets/images");

                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                    var filePath = Path.Combine(imagesFolder, fileName);
                    imageFile.SaveAs(filePath);

                    product.ImageURL = "/Assets/images/" + fileName;
                }
                db.Products.Add(product);
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