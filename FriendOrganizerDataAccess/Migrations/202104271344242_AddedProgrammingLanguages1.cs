namespace FriendOrganizerDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProgrammingLanguages1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Friend", "FavoriteLanguageId", "dbo.ProgrammingLanguage");
            DropIndex("dbo.Friend", new[] { "FavoriteLanguageId" });
            AlterColumn("dbo.Friend", "FavoriteLanguageId", c => c.Int());
            CreateIndex("dbo.Friend", "FavoriteLanguageId");
            AddForeignKey("dbo.Friend", "FavoriteLanguageId", "dbo.ProgrammingLanguage", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friend", "FavoriteLanguageId", "dbo.ProgrammingLanguage");
            DropIndex("dbo.Friend", new[] { "FavoriteLanguageId" });
            AlterColumn("dbo.Friend", "FavoriteLanguageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Friend", "FavoriteLanguageId");
            AddForeignKey("dbo.Friend", "FavoriteLanguageId", "dbo.ProgrammingLanguage", "Id", cascadeDelete: true);
        }
    }
}
