using ProjectShoeShop.DAL;
using ProjectShoeShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;

namespace ProjectShoeShop.ViewModel
{
    public class CartVM
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double Price { get; set; }
        public string ImageURL { get; set; }
        public string DiscountProduct { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public double Total
        {
            get { return Quantity * Price; }
        }
        ShoeShopDbContext db = new ShoeShopDbContext();
        public CartVM(string ProductId)
        {
            Id = ProductId;
            Product pro = db.Products.Where(x => x.Id == ProductId).FirstOrDefault();

            ProductName = pro.Name;
            ProductDescription = pro.Description;
            Price = double.Parse(pro.Price.ToString());
            Size = pro.Size;
            ImageURL = pro.ImageURL;
            Quantity = 1;
        }
        //public CartVM(string ProductId, string Name)
        //{
        //    Id = ProductId;
        //    Product pro = db.Products.Where(x => x.Name == Name && x.Id == ProductId).FirstOrDefault();

        //    ProductName = pro.Name;
        //    ProductDescription = pro.Description;
        //    Price = double.Parse(pro.Price.ToString());
        //    Size = pro.Size;
        //    ImageURL = pro.ImageURL;
        //    Quantity = 1;
        //}
    }
}