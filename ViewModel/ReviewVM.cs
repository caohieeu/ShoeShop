using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectShoeShop.ViewModel
{
    public class ReviewVM
    {
        public string FullName { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ImagePath { get; set; }
    }
}