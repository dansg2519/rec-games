namespace RecGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VersionWithoutPrice : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LanguageGame", "Game_GameID", "dbo.Game");
            DropForeignKey("dbo.TagGame", "Game_GameID", "dbo.Game");
            DropForeignKey("dbo.UserGame", "GameID", "dbo.Game");
            DropPrimaryKey("dbo.Game");
            AddColumn("dbo.Game", "TotalAchievements", c => c.Int(nullable: false));
            AddColumn("dbo.Game", "Recommendations", c => c.Int(nullable: false));
            AddColumn("dbo.Game", "MetacriticScore", c => c.Int(nullable: false));
            AlterColumn("dbo.Game", "GameID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Game", "GameID");
            AddForeignKey("dbo.LanguageGame", "Game_GameID", "dbo.Game", "GameID", cascadeDelete: true);
            AddForeignKey("dbo.TagGame", "Game_GameID", "dbo.Game", "GameID", cascadeDelete: true);
            AddForeignKey("dbo.UserGame", "GameID", "dbo.Game", "GameID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGame", "GameID", "dbo.Game");
            DropForeignKey("dbo.TagGame", "Game_GameID", "dbo.Game");
            DropForeignKey("dbo.LanguageGame", "Game_GameID", "dbo.Game");
            DropPrimaryKey("dbo.Game");
            AlterColumn("dbo.Game", "GameID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Game", "MetacriticScore");
            DropColumn("dbo.Game", "Recommendations");
            DropColumn("dbo.Game", "TotalAchievements");
            AddPrimaryKey("dbo.Game", "GameID");
            AddForeignKey("dbo.UserGame", "GameID", "dbo.Game", "GameID", cascadeDelete: true);
            AddForeignKey("dbo.TagGame", "Game_GameID", "dbo.Game", "GameID", cascadeDelete: true);
            AddForeignKey("dbo.LanguageGame", "Game_GameID", "dbo.Game", "GameID", cascadeDelete: true);
        }
    }
}
