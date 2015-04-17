using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecGames.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool Online { get; set; }
    }
}