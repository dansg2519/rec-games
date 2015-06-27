using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecGames.Models
{
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagID { get; set; }
        [Column("Tag")]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string TagName { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}