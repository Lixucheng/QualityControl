namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tradeuserid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GxTrade", "User_Id", "dbo.GxUser");
            DropIndex("dbo.GxTrade", new[] { "User_Id" });
            AddColumn("dbo.GxTrade", "UserId", c => c.String());
            DropColumn("dbo.GxTrade", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GxTrade", "User_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.GxTrade", "UserId");
            CreateIndex("dbo.GxTrade", "User_Id");
            AddForeignKey("dbo.GxTrade", "User_Id", "dbo.GxUser", "Id");
        }
    }
}
