namespace RecGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Game", "TotalAchievements", c => c.Int());
            AlterColumn("dbo.Game", "Recommendations", c => c.Int());
            AlterColumn("dbo.Game", "MetacriticScore", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Game", "MetacriticScore", c => c.Int(nullable: false));
            AlterColumn("dbo.Game", "Recommendations", c => c.Int(nullable: false));
            AlterColumn("dbo.Game", "TotalAchievements", c => c.Int(nullable: false));
        }
    }
}
