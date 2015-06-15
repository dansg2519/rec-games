namespace RecGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VersionWithoutNotNullables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Game", "PriceID", "dbo.Price");
            DropIndex("dbo.Game", new[] { "PriceID" });
            RenameColumn(table: "dbo.Game", name: "PriceID", newName: "Price_PriceID");
            AlterColumn("dbo.Game", "Price_PriceID", c => c.Int());
            CreateIndex("dbo.Game", "Price_PriceID");
            AddForeignKey("dbo.Game", "Price_PriceID", "dbo.Price", "PriceID");
            DropColumn("dbo.Game", "TotalAchievements");
            DropColumn("dbo.Game", "Recommendations");
            DropColumn("dbo.Game", "MetacriticScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Game", "MetacriticScore", c => c.Int(nullable: false));
            AddColumn("dbo.Game", "Recommendations", c => c.Int(nullable: false));
            AddColumn("dbo.Game", "TotalAchievements", c => c.Int(nullable: false));
            DropForeignKey("dbo.Game", "Price_PriceID", "dbo.Price");
            DropIndex("dbo.Game", new[] { "Price_PriceID" });
            AlterColumn("dbo.Game", "Price_PriceID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Game", name: "Price_PriceID", newName: "PriceID");
            CreateIndex("dbo.Game", "PriceID");
            AddForeignKey("dbo.Game", "PriceID", "dbo.Price", "PriceID", cascadeDelete: true);
        }
    }
}
