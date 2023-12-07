using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectShoeShop.Models
{
    public class Cart
    {
        [Key]
        public string Id { get; set; }
        public string UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public User User { get; set; }
        public string ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedDate { get; set; }
    }
}