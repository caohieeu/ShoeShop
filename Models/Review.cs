using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectShoeShop.Models
{
    public class Review
    {
        [Key]
        public string Id { get; set; }
        public string ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}