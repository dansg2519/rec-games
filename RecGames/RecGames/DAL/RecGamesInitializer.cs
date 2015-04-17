using RecGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecGames.DAL
{
    public class RecGamesInitializer
: System.Data.Entity.DropCreateDatabaseIfModelChanges<RecGameContext>
    {
        protected override void Seed(RecGameContext context)
        {
            var tags = new List<Tag>
            {
            new Tag{TagName="F"},
            new Tag{TagName="G"},
            new Tag{TagName="H"}
            };
            tags.ForEach(s => context.Tags.Add(s));
            context.SaveChanges();
        }
    }
}