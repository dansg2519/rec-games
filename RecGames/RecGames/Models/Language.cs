using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecGames.Models
{
    public class Language
    {
        [Key]
        public int LanguageID { get; set; }
        public string Audio { get; set; }
        public string Interface { get; set; }
        public string Subtitle { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}