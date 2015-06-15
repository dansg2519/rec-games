namespace RecGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Game", "TotalAchievements", c => c.Int(nullable: false));
            AlterColumn("dbo.Game", "Recommendations", c => c.Int(nullable: false));
            AlterColumn("dbo.Game", "MetacriticScore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Game", "MetacriticScore", c => c.Int());
            AlterColumn("dbo.Game", "Recommendations", c => c.Int());
            AlterColumn("dbo.Game", "TotalAchievements", c => c.Int());
        }
    }
}
