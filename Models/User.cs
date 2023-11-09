using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectShoeShop.Models
{
    public class User
    {
        public string Id { get ; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime? Birth { get; set; }
        public string Phone { get; set; }
        public string ImagePath { get; set; }
        public User()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 7);
        }
    }
}
