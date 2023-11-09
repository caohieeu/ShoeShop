using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectShoeShop.Models
{
    public class ProductImage
    {
        [Key]
        public string Id { get; set; }
        public string ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }
        public string ImageURL { get; set; }
    }
}   