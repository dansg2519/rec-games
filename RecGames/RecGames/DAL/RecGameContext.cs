using RecGames.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace RecGames.DAL
{
    public class RecGameContext : DbContext
    {

        public RecGameContext()
            : base("RecGameContext")
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Tag> Tags { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}
    }
}