using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyASPBackend.Models
{
    public class LoginData
    {
        /// <summary>
        /// Konto, na ktore chcemy sie zalogowac
        /// </summary>
        public string Nick { get; set; }
        /// <summary>
        /// Haslo do logowania.
        /// </summary>
        public string Password { get; set; }
    }
}