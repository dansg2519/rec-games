using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecGames.Models
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty(PropertyName = "appid")]
        public int GameID { get; set; }
        public string Name { get; set; }
        public string ControllersSupported { get; set; }
        public string Platforms { get; set; }
        public string Developers { get; set; }
        public string Publishers { get; set; }
        public string Genre { get; set; }
        public string LaunchDate { get; set; }
        public string HeaderImage { get; set; }
        public int TotalAchievements { get; set; }
        public int Recommendations { get; set; }
        public int MetacriticScore { get; set; }
        public string PriceCurrency { get; set; }
        public double PriceValue { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<UserGame> UsersGames { get; set; }

        public Game() { }
        public Game(int GameID, string Name, int MetacriticScore, ICollection<Tag> Tags, int Recommendations)
        {
            this.GameID = GameID;
            this.Name = Name;
            this.MetacriticScore = MetacriticScore;
            this.Recommendations = Recommendations;
            this.Tags = Tags;
        }
    }
}