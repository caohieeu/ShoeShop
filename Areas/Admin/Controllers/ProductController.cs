using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using ProjectShoeShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ProjectShoeShop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ShoeShopDbContext db = new ShoeShopDbContext();

        // GET: Admin/Product
        public ActionResult Index(String Search = "", String IconClass = "fa-sort-asc",
            String SortColumn = "DateCreated", int Page = 1)
        {
            List<Product> listProduct = db.Products
                .Include(m => m.Brand)
                .Include(m => m.Category)
                .ToList();
            if(listProduct != null)
            {
                //Search
                if (!string.IsNullOrEmpty(Search))
                {
                    listProduct = db.Products.Where(m => m.Name.Contains(Search)).ToList();
                    ViewBag.Search = Search;
                }

                //sort
                ViewBag.IconClass = IconClass;
                ViewBag.SortColumn = SortColumn;
                if(SortColumn == "Id")
                {
                    if(IconClass == "fa-sort-asc")
                    {
                        listProduct = listProduct.OrderBy(m => m.Id).ToList();
                    }
                    else
                    {
                        listProduct = listProduct.OrderByDescending(m => m.Id).ToList();
                    }
                }
                else if (SortColumn == "Name")
                {
                    if (IconClass == "fa-sort-asc")
                    {
                        listProduct = listProduct.OrderBy(m => m.Name).ToList();
                    }   
                    else
                    {
                        listProduct = listProduct.OrderByDescending(m => m.Size).ToList();
                    }
                }
                else if (SortColumn == "Size")
                {
                    if (IconClass == "fa-sort-asc")
                    {
                        listProduct = listProduct.OrderBy(m => m.Size).ToList();
                    }
                    else
                    {
                        listProduct = listProduct.OrderByDescending(m => m.Size).ToList();
                    }
                }
                else if (SortColumn == "Price")
                {
                    if (IconClass == "fa-sort-asc")
                    {
                        listProduct = listProduct.OrderBy(m => m.Price).ToList();
                    }
                    else
                    {
                        listProduct = listProduct.OrderByDescending(m => m.Price).ToList();
                    }
                }
                else if (SortColumn == "Stock")
                {
                    if (IconClass == "fa-sort-asc")
                    {
                        listProduct = listProduct.OrderBy(m => m.Stock).ToList();
                    }
                    else
                    {
                        listProduct = listProduct.OrderByDescending(m => m.Stock).ToList();
                    }
                }
                else if(SortColumn == "DateCreated")
                {
                    listProduct = listProduct.OrderBy(m => m.Stock).ToList();
                }
                ViewBag.ProductNumber = listProduct.Count;
                //Paging
                int NumberOfRecords = 10;
                int NumberOfPages = Convert.ToInt32(Math.Ceiling(listProduct.Count / (1.0 * NumberOfRecords)));
                int NumberOfRecordToSkip = (Page - 1) * NumberOfRecords;
                listProduct = listProduct.Skip(NumberOfRecordToSkip).Take(NumberOfRecords).ToList();
                ViewBag.NumberOfPages = NumberOfPages;
                ViewBag.Page = Page;

                return View(listProduct);
            }
            return HttpNotFound();
        }

        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductVM obj , HttpPostedFileBase ImageURL)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Id = Guid.NewGuid().ToString().Substring(0, 7),
                    Name = obj.Name,
                    BrandID = obj.BrandID,
                    CategoryID = obj.CategoryID,
                    Description = obj.Description,
                    Price = Decimal.Parse(obj.Price.ToString()),
                    Size = obj.Size,
                    Stock = obj.Stock,
                    GenderShoe = obj.GenderShoe,
                    DateCreated = DateTime.Now,
                    DatePurchase = null
                };
                if (ImageURL != null && ImageURL.ContentLength > 0)
                {
                    var imagesFolder = Server.MapPath("~/Assets/images");

                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageURL.FileName);

                    var filePath = Path.Combine(imagesFolder, fileName);
                    ImageURL.SaveAs(filePath);

                    product.ImageURL = "/Assets/images/" + fileName;
                }
                db.Products.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Categories = db.Categories.ToList();
                ViewBag.Brands = db.Brands.ToList();
                return View();
            }
        }
        public ActionResult CreateNewSize(string id)
        {
            ProductVM product = db.Products
                     .Where(pro => pro.Id == id)
                     .Select(pro => new ProductVM()
                     {
                         Id = pro.Id,
                         Name = pro.Name,
                         Description = pro.Description,
                         Size = pro.Size,
                         Price = pro.Price,
                         Stock = pro.Stock,
                         GenderShoe = pro.GenderShoe,
                         ImageURL = pro.ImageURL,
                         CategoryID = pro.CategoryID,
                         BrandID = pro.BrandID,
                     }).FirstOrDefault();

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            ViewBag.IdProduct = product.Id;
            if (product != null)
            {
                return View(product);
            }
            return View(product);
        }
        [HttpPost]
        public ActionResult CreateNewSize(string id, ProductVM obj)
        {
            var existingProduct = db.Products.FirstOrDefault(x => x.Id == id);
            var product = new Product()
            {
                Id = Guid.NewGuid().ToString().Substring(0, 7),
                Name = existingProduct.Name,
                BrandID = existingProduct.BrandID,
                CategoryID = existingProduct.CategoryID,
                Description = existingProduct.Description,
                Price = Decimal.Parse(obj.Price.ToString()),
                Size = obj.Size,
                Stock = obj.Stock,
                GenderShoe = existingProduct.GenderShoe,
                DateCreated = DateTime.Now,
                DatePurchase = null,
                ImageURL = existingProduct.ImageURL
            };
            db.Products.Add(product);
            db.SaveChanges();

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string id)
        {
            ProductVM product = db.Products
                     .Where(pro => pro.Id == id)
                     .Select(pro => new ProductVM()
                     {
                         Id = pro.Id,
                         Name = pro.Name,
                         Description = pro.Description,
                         Size = pro.Size,
                         Price = pro.Price,
                         Stock = pro.Stock,
                         GenderShoe = pro.GenderShoe,   
                         ImageURL = pro.ImageURL,
                         CategoryID = pro.CategoryID,
                         BrandID = pro.BrandID,
                     }).FirstOrDefault();

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            ViewBag.IdProduct = product.Id;
            if (product != null)
            {
                return View(product);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(ProductVM obj, string id, HttpPostedFileBase ImageURL)
        {

            if (ModelState.IsValid)
            {
                var existingProduct = db.Products.FirstOrDefault(x => x.Id == id);

                if (existingProduct != null)
                {
                    existingProduct.Id = id;
                    existingProduct.Name = obj.Name;
                    existingProduct.Description = obj.Description;
                    existingProduct.Price = obj.Price;
                    existingProduct.Stock = obj.Stock;
                    existingProduct.GenderShoe = obj.GenderShoe;
                    existingProduct.Size = obj.Size;
                    existingProduct.BrandID = obj.BrandID;
                    existingProduct.CategoryID = obj.CategoryID;
                    existingProduct.DateCreated = DateTime.Now;

                    if (ImageURL != null && ImageURL.ContentLength > 0)
                    {
                        if(!string.IsNullOrEmpty(existingProduct.ImageURL))
                        {
                            var oldImagePath = Server.MapPath(existingProduct.ImageURL);
                            if(System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        var imagesFolder = Server.MapPath("~/Assets/images");

                        if (!Directory.Exists(imagesFolder))
                        {
                            Directory.CreateDirectory(imagesFolder);
                        }

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageURL.FileName);

                        var filePath = Path.Combine(imagesFolder, fileName);
                        ImageURL.SaveAs(filePath);

                        existingProduct.ImageURL = "/Assets/images/" + fileName;
                    }
                    db.Entry(existingProduct).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View(obj);
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
        public ActionResult ParitalViewProduct()
        {
            List<Category> listCategories = db.Categories.ToList();
            return PartialView(listCategories);
        }
        public ActionResult DetailProduct(string id)
        {
            ProductAPIController api = new ProductAPIController();
            Product pro = api.getDetailProduct(id);
            if(pro != null)
            {
                string Brand = db.Brands.Where(x => x.Id == pro.BrandID).Select(x => x.Name).FirstOrDefault().ToString();
                string Category = db.Categories.Where(x => x.Id == pro.CategoryID).Select(x => x.Name).FirstOrDefault().ToString();
                ViewBag.Brand = Brand;
                ViewBag.Category = Category;
                return View(pro);
            }
            return HttpNotFound();
        }
        public List<string> GetAvailableSizes(string ProductName)
        {
            List<string> lstSize = new List<string> { "SMALL", "MEDIUM", "LARGE", "EXTRA LARGE" };
            List<Product> lstPro = db.Products.Where(x => x.Name == ProductName).ToList();
            foreach(var item in lstPro)
            {
                lstSize.Remove(item.Size);
            }
            return lstSize;
        }
    }
}