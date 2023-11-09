using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectShoeShop.Models
{
    public class Role
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set;}
    }
}