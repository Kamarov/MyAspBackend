using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyASPBackend.Models
{
    /// <summary>
    /// Klasa do rejestrowania Userow
    /// </summary>
    public class Registration
    {
        
        [Required(ErrorMessage ="Nick is requaried!")]
        [MinLength(5)]
        public string Nick { get; set;}
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail is requaried!")]
        [EmailAddress(ErrorMessage = "Invalid Adress!")]
        public string Email { get; set;}
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is requaried!")]
        [MinLength(10)]
        public string Password { get; set;}
    }
}