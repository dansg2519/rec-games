namespace RecGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        GameID = c.Int(nullable: false),
                        Name = c.String(),
                        ControllersSupported = c.String(),
                        Platforms = c.String(),
                        Developers = c.String(),
                        Publishers = c.String(),
                        Genre = c.String(),
                        LaunchDate = c.String(),
                        TotalAchievements = c.Int(nullable: false),
                        Recommendations = c.Int(nullable: false),
                        MetacriticScore = c.Int(nullable: false),
                        PriceCurrency = c.String(),
                        PriceValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.GameID);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        LanguageID = c.Int(nullable: false, identity: true),
                        Audio = c.String(),
                        Interface = c.String(),
                        Subtitle = c.String(),
                    })
                .PrimaryKey(t => t.LanguageID);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        Tag = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TagID)
                .Index(t => t.Tag, unique: true);
            
            CreateTable(
                "dbo.UserGame",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        GameID = c.Int(nullable: false),
                        TotalHoursPlayed = c.Int(nullable: false),
                        LastTimePlayed = c.String(),
                        AchievementsObtained = c.String(),
                    })
                .PrimaryKey(t => new { t.UserID, t.GameID })
                .ForeignKey("dbo.Game", t => t.GameID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.GameID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        RealName = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        Online = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.LanguageGame",
                c => new
                    {
                        Language_LanguageID = c.Int(nullable: false),
                        Game_GameID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Language_LanguageID, t.Game_GameID })
                .ForeignKey("dbo.Language", t => t.Language_LanguageID, cascadeDelete: true)
                .ForeignKey("dbo.Game", t => t.Game_GameID, cascadeDelete: true)
                .Index(t => t.Language_LanguageID)
                .Index(t => t.Game_GameID);
            
            CreateTable(
                "dbo.GameTag",
                c => new
                    {
                        GameID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GameID, t.TagID })
                .ForeignKey("dbo.Game", t => t.GameID, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagID, cascadeDelete: true)
                .Index(t => t.GameID)
                .Index(t => t.TagID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGame", "UserID", "dbo.User");
            DropForeignKey("dbo.UserGame", "GameID", "dbo.Game");
            DropForeignKey("dbo.GameTag", "TagID", "dbo.Tag");
            DropForeignKey("dbo.GameTag", "GameID", "dbo.Game");
            DropForeignKey("dbo.LanguageGame", "Game_GameID", "dbo.Game");
            DropForeignKey("dbo.LanguageGame", "Language_LanguageID", "dbo.Language");
            DropIndex("dbo.GameTag", new[] { "TagID" });
            DropIndex("dbo.GameTag", new[] { "GameID" });
            DropIndex("dbo.LanguageGame", new[] { "Game_GameID" });
            DropIndex("dbo.LanguageGame", new[] { "Language_LanguageID" });
            DropIndex("dbo.UserGame", new[] { "GameID" });
            DropIndex("dbo.UserGame", new[] { "UserID" });
            DropIndex("dbo.Tag", new[] { "Tag" });
            DropTable("dbo.GameTag");
            DropTable("dbo.LanguageGame");
            DropTable("dbo.User");
            DropTable("dbo.UserGame");
            DropTable("dbo.Tag");
            DropTable("dbo.Language");
            DropTable("dbo.Game");
        }
    }
}
