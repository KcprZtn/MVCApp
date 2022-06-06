using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LearningMVC.Models
{
    public class LoginModel
    {
   
        public int userId { get; set; }

        [Required(ErrorMessage = "Please enter login.")]
        [DisplayName("Login")]
        public string Login { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Your password needs to be 3-100 characters long.")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Please repeat your password.")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Your passwords aren't the same.")]

        public string ConfirmPassword { get; set; }

    }
}
