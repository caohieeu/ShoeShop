using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectShoeShop.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Username cannot be blank")]
        [RegularExpression(@"^[a-zA-Z 0-9]*$", ErrorMessage = "Cannot use special characters in username")]
        [MinLength(5, ErrorMessage = "Username should contain at least 5 characters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password cannot be blank")]
        [MinLength(6, ErrorMessage = "Password should contain at least 6 characters")]
        public string Password { get; set; }
    }
}