using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectShoeShop.ViewModel
{
    public class OrderVM
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ImageProduct { get; set; }
        public string NameProduct { get; set; }
        public string DeliveryStatus { get; set; }
        public int QuantityProduct { get; set; }
        public double TotalAmount { get; set; }
    }
}