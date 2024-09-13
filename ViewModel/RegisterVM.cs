using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace ProjectShoeShop.ViewModel
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "FullName cannot be blank")]
        [RegularExpression(@"^[a-z A-Z]*$", ErrorMessage = "Cannot use special characters in fullname")]
        [MinLength(5, ErrorMessage = "FullName should contain at least 5 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username cannot be blank")]
        [RegularExpression(@"^[a-zA-Z 0-9]*$", ErrorMessage = "Cannot use special characters in username")]
        [MinLength(5, ErrorMessage = "Username should contain at least 5 characters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password cannot be blank")]
        [MinLength(6, ErrorMessage = "Password should contain at least 6 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password cannot be blank")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }
        [RegularExpression(@"^(0[1-9])+[0-9]*$", ErrorMessage = "Invalid phone number")]
        [MinLength(10, ErrorMessage = "Phone number should contain at least 10 characters")]
        public string Phone { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Birth { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }

    }
}