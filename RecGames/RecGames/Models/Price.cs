using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecGames.Models
{
    public class Price
    {
        [Key]
        public int PriceID { get; set; }
        public string Currency { get; set; }
        public double Value { get; set; }
    }
}