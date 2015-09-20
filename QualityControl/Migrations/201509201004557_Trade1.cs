namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Trade1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxTrade", "Manufacturer_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.GxTrade", "SgsUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.GxTrade", "Manufacturer_Id");
            CreateIndex("dbo.GxTrade", "SgsUser_Id");
            AddForeignKey("dbo.GxTrade", "Manufacturer_Id", "dbo.GxUser", "Id");
            AddForeignKey("dbo.GxTrade", "SgsUser_Id", "dbo.GxUser", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GxTrade", "SgsUser_Id", "dbo.GxUser");
            DropForeignKey("dbo.GxTrade", "Manufacturer_Id", "dbo.GxUser");
            DropIndex("dbo.GxTrade", new[] { "SgsUser_Id" });
            DropIndex("dbo.GxTrade", new[] { "Manufacturer_Id" });
            DropColumn("dbo.GxTrade", "SgsUser_Id");
            DropColumn("dbo.GxTrade", "Manufacturer_Id");
        }
    }
}
