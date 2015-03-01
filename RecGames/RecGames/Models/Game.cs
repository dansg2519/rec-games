using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecGames.Models
{
    public class Game
    {
        [Key]
        public int GameID { get; set; }
        public string Name { get; set; }
        public string ControllerSupport { get; set; }
        public string Platforms { get; set; }
        public string Developers { get; set; }
        public string Publishers { get; set; }
        public int Recommendations { get; set; }
        public int MetacriticScore { get; set; }
        public int PriceID { get; set; }
    }
}