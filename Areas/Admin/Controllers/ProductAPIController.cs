using ProjectShoeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjectShoeShop.DAL;

namespace ProjectShoeShop.Areas.Admin.Controllers
{
    public class ProductAPIController : ApiController
    {
        ShoeShopDbContext db = new ShoeShopDbContext();
        public List<Product> getListProduct()
        {
            List<Product> listProduct = (from p in db.Products
                                         select p).ToList();
            return listProduct;
        }
        public Product getDetailProduct(string id)
        {
            Product product = (from p in db.Products
                               where p.Id == id
                               select p).FirstOrDefault();
            return product;
        }
    }
}
