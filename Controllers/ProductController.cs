using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using ProjectShoeShop.ViewModel;

namespace ProjectShoeShop.Controllers
{
    public class ProductController : Controller
    {
        ShoeShopDbContext db = new ShoeShopDbContext();
        // GET: Product
        public ActionResult ProductLayout()
        {
            return View();
        }
        public ActionResult ShowProduct(int MinRange = 0, int MaxRange = 100000000)   
        {
            var listProduct = db.Products.Include(nameof(Product.Brand))
                                         .Include(nameof(Product.Category))
                                         .Take(10).ToList();
            listProduct = listProduct.Where(x => x.Price >= MinRange && x.Price <= MaxRange).ToList();
            ViewBag.MinRange = MinRange;
            ViewBag.MaxRange = MaxRange;
            if (listProduct != null)
            {
                return View(listProduct);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult DetailProduct(string id, string ProductName)
        {
            var lst = db.Products.Where(t=>t.Name == ProductName).Include(nameof(Product.Brand)).FirstOrDefault();
            ViewBag.CountReviews = CountRating(id);
            ViewBag.FiveStarReviews = CountFiveRating(id);
            ViewBag.FourStarReviews = CountFourRating(id);
            ViewBag.ThreeStarReviews = CountThreeRating(id);
            ViewBag.TwoStarReviews = CountTwoRating(id);
            ViewBag.OneStarReviews = CountOneRating(id);
            ViewBag.AvgReviews = AVGRating(id);
            ViewBag.GetProductRanking = GetProductRanking(id);
            ViewBag.StockSizeSmall = db.Products.Where(x => x.Id == id && x.Size == "SMALL").Select(x => x.Stock).FirstOrDefault();
            return View(lst);
        }
        public ActionResult FilterByMenPartial()
        {
            var listCategories = db.Categories.ToList();
            try
            {
                return View(listCategories);
            }
            catch
            {
                return View();
            }
        }
        public ActionResult FilterByWomenPartial()
        {
            var listCategories = db.Categories.ToList();
            try
            {
                return View(listCategories);
            }
            catch
            {
                return View();
            }
        }
        public ActionResult ProductByMenCategory(string id, int MinRange = 0, int MaxRange = 100000000)
        {
            try
            {
                List<Product> listProducts = db.Products
                                            .Include(nameof(Product.Category))
                                            .Where(x => x.CategoryID == id && x.GenderShoe=="male").ToList();
                if(listProducts != null)
                {
                    listProducts = listProducts.Where(x => x.Price >= MinRange && x.Price <= MaxRange).ToList();
                    ViewBag.MinRange = MinRange;
                    ViewBag.MaxRange = MaxRange;
                    return View(listProducts);
                }
                return View();
            }
            catch
            {
                return HttpNotFound();
            }
        }
        public ActionResult ProductByWomenCategory(string id, int MinRange = 0, int MaxRange = 100000000)
        {
            try
            {
                List<Product> listProducts = db.Products
                                            .Include(nameof(Product.Category))
                                            .Where(x => x.CategoryID == id && x.GenderShoe == "female").ToList();
                if (listProducts != null)
                {
                    listProducts = listProducts.Where(x => x.Price >= MinRange && x.Price <= MaxRange).ToList();
                    ViewBag.MinRange = MinRange;
                    ViewBag.MaxRange = MaxRange;
                    return View(listProducts);
                }
                return View();
            }
            catch
            {
                return HttpNotFound();
            }
        }
        public int GetStockBySize(string Size, string ProductName)
        {
            int stock = 0;
            stock = db.Products
                        .Where(x => x.Name == ProductName && x.Size == Size)
                        .Select(x => x.Stock).FirstOrDefault();
            return stock;
        }
        public ActionResult ReviewPartial(string productID)
        {
            var lst = db.Reviews.Include(nameof(Review.User))
                    .Include(nameof(Review.Product)).Where(t => t.ProductId == productID)
                    .Select(t => new ReviewVM
                    {
                        FullName = t.User.FullName,
                        ImagePath = t.User.ImagePath,
                        Comment =  t.Comment,
                        Rating = t.Rating,
                        ReviewDate = t.ReviewDate
                    })
                    .ToList();
            if(lst != null)
            {             
                return PartialView(lst);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public int CountRating(string id)
        {
            var lst = db.Reviews.Count(t => t.ProductId == id);
            return lst;
        }
        public int CountFiveRating(string id)
        {
            var lst = db.Reviews.Count(t=>t.ProductId == id && t.Rating == 5);
            return lst;
        }
        public int CountFourRating(string id)
        {
            var lst = db.Reviews.Count(t => t.ProductId == id && t.Rating == 4);
            return lst;
        }
        public int CountThreeRating(string id)
        {
            var lst = db.Reviews.Count(t => t.ProductId == id && t.Rating == 3);
            return lst;
        }
        public int CountTwoRating(string id)
        {
            var lst = db.Reviews.Count(t => t.ProductId == id && t.Rating == 2);
            return lst;
        }
        public int CountOneRating(string id)
        {
            var lst = db.Reviews.Count(t => t.ProductId == id && t.Rating == 1);
            return lst;
        }
        public int AVGRating(string id)
        {
            var ratings = db.Reviews.Where(t => t.ProductId == id).Select(t => t.Rating);
            if (ratings.Any())
            {
                return (int)ratings.Average();
            }
            return 0;
        }
        public int GetProductRanking(string productId)
        {
            try
            {
                var productRating = db.Reviews
                .Where(r => r.ProductId == productId)
                .Average(r => r.Rating);

                var productRankings = db.Reviews
                    .GroupBy(r => r.ProductId)
                    .Select(grp => new
                    {
                        ProductId = grp.Key,
                        AverageRating = grp.Average(r => r.Rating)
                    })
                    .OrderByDescending(x => x.AverageRating)
                    .ToList();

                var ranking = productRankings.FindIndex(x => x.ProductId == productId) + 1;

                return ranking;
            }
            catch
            {
                return 0;
            }
        }

    }
}