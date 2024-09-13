using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using ProjectShoeShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectShoeShop.Controllers
{
    public class AboutController : Controller
    {
        ShoeShopDbContext db = new ShoeShopDbContext();
        // GET: About
        public ActionResult AboutUs()
        {
            ViewBag.Random = GetRanDomReviews();

            return View();
        }
        public ActionResult AboutLayout()
        {
            return View();
        }
        public string GetRanDomReviews()
        {
            var random = db.Reviews.Where(t => t.Rating == 5).OrderBy(t => Guid.NewGuid()).Select(t => t.Comment).FirstOrDefault();
            return random;
        }
        public ActionResult ReviewPartial()
        {
            var lst = db.Reviews.Include(nameof(Review.User))
                    .Include(nameof(Review.Product)).Where(t => t.Rating == 5)
                    .Select(t => new ReviewVM
                    {
                        FullName = t.User.FullName,
                        ImagePath = t.User.ImagePath,
                        Comment = t.Comment,
                        Rating = t.Rating,
                        ReviewDate = t.ReviewDate
                    })
                    .ToList();
            if (lst != null)
            {
                return PartialView(lst);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}