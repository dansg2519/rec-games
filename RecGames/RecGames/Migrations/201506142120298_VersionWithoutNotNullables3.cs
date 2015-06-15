namespace RecGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VersionWithoutNotNullables3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Game", "Recommendations", c => c.Int(nullable: false));
            AddColumn("dbo.Game", "MetacriticScore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Game", "MetacriticScore");
            DropColumn("dbo.Game", "Recommendations");
        }
    }
}
