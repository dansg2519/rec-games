using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecGames.Models
{
    public class UserGame
    {
        [Key][Column(Order = 0)]
        public int UserID { get; set; }
        [Key][Column(Order = 1)]
        public int GameID { get; set; }
        public int TotalHoursPlayed { get; set; }
        public string LastTimePlayed { get; set; }
        public string AchievementsObtained { get; set; }

        public virtual Game Games { get; set; }
        public virtual User Users { get; set; }
    }
}