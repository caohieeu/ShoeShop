using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectShoeShop.ViewModel
{
    public class NewOrdersVM
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string ProductId { get; set; }
        public string NameProduct { get; set; }
        public string DeliveryStatus { get; set; }
        public string ImagePath { get; set; }
        public int QuantityProduct { get; set; }
    }
}