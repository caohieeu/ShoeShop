using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ProjectShoeShop.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; }
        public string UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public User User { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderStatus { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime OrderDate { get; set; }
    }
}