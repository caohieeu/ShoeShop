using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        public ActionResult ShowProduct()
        {
            return View();
        }
        public ActionResult DetailProduct(string id)
        {
            var lst = db.Products.Where(t=>t.Id == id).Include(nameof(Product.Brand)).FirstOrDefault();
            ViewBag.CountReviews = CountRating(id);
            ViewBag.FiveStarReviews = CountFiveRating(id);
            ViewBag.FourStarReviews = CountFourRating(id);
            ViewBag.ThreeStarReviews = CountThreeRating(id);
            ViewBag.TwoStarReviews = CountTwoRating(id);
            ViewBag.OneStarReviews = CountOneRating(id);
            ViewBag.AvgReviews = AVGRating(id);
            ViewBag.GetProductRanking = GetProductRanking(id);
            return View(lst);
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
            return 5;
        }
        public int GetProductRanking(string productId)
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

    }
}