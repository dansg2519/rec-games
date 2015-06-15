using RecGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Configuration;

namespace RecGames.DAL
{
    public class RecGamesInitializer
: System.Data.Entity.DropCreateDatabaseIfModelChanges<RecGameContext>
    {
        protected override void Seed(RecGameContext context)
        {
            
        }
    }
}