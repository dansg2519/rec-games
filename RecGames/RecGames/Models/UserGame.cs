using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecGames.Models
{
    public class UserGame
    {
        [Key]
        public int UserID { get; set; }
        [Key]
        public int GameID { get; set; }
        public int TotalHoursPlayed { get; set; }
        public string LastTimePlayed { get; set; }
        public string AchievementsObtained { get; set; }
    }
}