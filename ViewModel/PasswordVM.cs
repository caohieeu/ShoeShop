using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectShoeShop.ViewModel
{
    public class PasswordVM
    {

        [Required(ErrorMessage = "New password cannot be blank")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New password cannot be blank")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm password cannot be blank")]
        [Compare("NewPassword", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set;}
    }
}