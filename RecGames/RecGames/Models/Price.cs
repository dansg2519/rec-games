using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecGames.Models
{
    //esse modelo pode ter problemas se considerarmos descontos.
    public class Price
    {
        [Key]
        public int PriceID { get; set; }
        public string Currency { get; set; }
        public double Value { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}