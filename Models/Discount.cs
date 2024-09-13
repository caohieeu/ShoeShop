using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectShoeShop.Models
{
    public class Discount
    {
        [Key]
        public string Id { get; set; }

        public string Code { get; set; }
        public string ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; }    
        public DateTime? EndDate { get; set; }
    }
}