namespace RecGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGameHeaderImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Game", "HeaderImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Game", "HeaderImage");
        }
    }
}
