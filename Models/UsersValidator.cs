using System;
using System.ComponentModel.DataAnnotations;


namespace CoderHelp.Models
{
    public class UsersValidator
    {   
        [Required(ErrorMessage= "Username is a required field!")]
        [MinLength(2)]
        public string Username { get; set; }

        [Required(ErrorMessage= "Email is a required field!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage= "Password is a required field!")]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage="Password is a required field!")]
        [DataType(DataType.Password)]
        [CompareAttribute("Password")]
        public string Confirm_Password {get; set;}

        public string Employment { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Bio { get; set; }

    }



}