using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LearningMVC.Models
{
    public class LoginModel
    {
   
        [Required(ErrorMessage = "Please enter login.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Your password needs to be 6-100 characters long.")]
        public string Password { get; set; }

    }
}
