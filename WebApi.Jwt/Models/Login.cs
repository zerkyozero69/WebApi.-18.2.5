using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Jwt.Models
{
    public class Login
    {
        public string Username { get; set; }
        public string Password  { get;set; }
          public  object resultLogin { get; set; }

    }
    public class Reset_Password
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class UserModel
    {
        [Required]
        [Display(Name = "userName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string Password { get; set; }
        public string comfig { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirmPassword")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }



}
